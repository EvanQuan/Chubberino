using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chubberino.Bots.Channel.Modules.CheeseGame.Emotes;
using Chubberino.Common.Extensions;
using Chubberino.Common.Services;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client.TwitchClients;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Heists;

public sealed class HeistManager : IHeistManager
{
    public static class Messages
    {
        public const String NoHeistExists = "There currently is no heist to join in this channel.";
        public const String HowToJoin = "To join the heist, you must wager some amount of cheese \"!cheese heist <cheese to wager>\". " +
                "The more you wager, the greater the risk and reward.";
        public const String NewHeistInitiated = "A heist has been initiated. " +
                "You and others can join with \"!cheese heist <cheese amount>\". " +
                "The more cheese wagered, the greater the risk and reward! " +
                "The heist will begin in {0}.";
        public const String Wait = "You must wait {0} until you can initiate another heist. {1}";
        public const String InvalidWager = "Cannot wager \"{0}\". You must wager a positive number of cheese to join the heist.";
    }

    private static TimeSpan HeistWaitTime { get; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Key: channel that the heist was initiated in.
    /// Value: The ongoing heist.
    /// </summary>
    private ConcurrentDictionary<String, IHeist> OngoingHeists { get; }

    public static TimeSpan HeistCooldown { get; set; } = TimeSpan.FromHours(6);
    public IApplicationContextFactory ContextFactory { get; }
    public ITwitchClientManager Client { get; }
    public Random Random { get; }
    public IEmoteManager EmoteManager { get; }
    public IThreadService ThreadService { get; }
    public IDateTimeService DateTime { get; }

    public HeistManager(
        IApplicationContextFactory contextFactory,
        ITwitchClientManager client,
        Random random,
        IEmoteManager emoteManager,
        IThreadService threadService,
        IDateTimeService dateTime)
    {
        OngoingHeists = new ConcurrentDictionary<String, IHeist>();
        ContextFactory = contextFactory;
        Client = client;
        Random = random;
        EmoteManager = emoteManager;
        ThreadService = threadService;
        DateTime = dateTime;
    }

    public void Heist(ChatMessage message)
    {
        using var context = ContextFactory.GetContext();

        var player = context.GetPlayer(Client, message);

        // Find if there's another ongoing heist in this channel to join.
        if (OngoingHeists.TryGetValue(message.Channel, out IHeist currentHeist))
        {
            JoinHeist(message, player, context);
            return;
        }

        // Run in a separate thread as it involves sleeping to wait for joiners.
        Task.Run(() => InitiateNewHeist(message));
    }

    private void InitiateNewHeist(ChatMessage message)
    {
        using var context = ContextFactory.GetContext();

        var player = context.GetPlayer(Client, message);

        // Trying to initiate new heist
        var now = DateTime.Now;

        var oldLastHeistInitiated = player.LastHeistInitiated;

        var timeSinceLastHeistInitiated = now - oldLastHeistInitiated;

        if (timeSinceLastHeistInitiated >= HeistCooldown)
        {
            // Update last heist initiated to prevent initiating
            // multiple heists in multiple channels.
            player.LastHeistInitiated = now;
            context.SaveChanges();

            var heist = new Heist(message, Random, Client);
            OngoingHeists.TryAdd(message.Channel, heist);

            Client.SpoolMessageAsMe(message.Channel, player,
                Messages.NewHeistInitiated.Format(HeistWaitTime.Format()));


            // Join the heist in a separate thread so that the heist
            // countdown can start immediately as the join is being
            // processed. This should only save around a second or so of
            // time.
            Task.Run(() => JoinHeist(message, player, context));

            // Since we are sleeping, this needs to be async.
            ThreadService.Sleep(HeistWaitTime);

            if (!heist.Start(context))
            {
                // No one joined the heist. Let the user initiate another heist.
                player.LastHeistInitiated = oldLastHeistInitiated;
                context.SaveChanges();
            }

            OngoingHeists.TryRemove(message.Channel, out _);
            return;
        }

        var timeUntilNextHeistAvailable = HeistCooldown - timeSinceLastHeistInitiated;

        var timeToWait = timeUntilNextHeistAvailable.Format();

        Client.SpoolMessageAsMe(message.Channel, player,
            Messages.Wait.Format(timeToWait, Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Waiting))));

    }

    private void JoinHeist(ChatMessage message, Player player, IApplicationContext context)
    {
        if (!OngoingHeists.TryGetValue(message.Channel, out IHeist heist))
        {
            Client.SpoolMessageAsMe(message.Channel, player, Messages.NoHeistExists);
            return;
        }

        String strippedCommand = message.Message.GetNextWord(out _).GetNextWord(out _);

        if (String.IsNullOrWhiteSpace(strippedCommand))
        {
            Client.SpoolMessageAsMe(message.Channel, player, Messages.HowToJoin);
            return;
        }

        strippedCommand.GetNextWord(out String proposedWager);

        if (proposedWager.TryGetWager(out var wager))
        {
            heist.UpdateWager(context, player, wager);
            return;
        }

        Client.SpoolMessageAsMe(message.Channel, player, Messages.InvalidWager.Format(proposedWager));
    }

    public void LeaveAllHeists(IApplicationContext context, Player player)
    {
        IEnumerable<IHeist> heists = OngoingHeists
            .Where(x => x.Value.Wagers.Any(x => x.PlayerTwitchID == player.TwitchUserID))
            .Select(x => x.Value);

        foreach (var heist in heists)
        {
            heist.UpdateWager(context, player, p => 0, silent: true);
        }
    }
}
