using Chubberino.Client;
using Chubberino.Client.Services;
using Chubberino.Client.Threading;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public sealed class HeistManager : IHeistManager
    {
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
        public ISpinWait SpinWait { get; }
        public IDateTimeService DateTime { get; }

        public HeistManager(
            IApplicationContextFactory contextFactory,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager,
            ISpinWait spinWait,
            IDateTimeService dateTime)
        {
            OngoingHeists = new ConcurrentDictionary<String, IHeist>();
            ContextFactory = contextFactory;
            Client = client;
            Random = random;
            EmoteManager = emoteManager;
            SpinWait = spinWait;
            DateTime = dateTime;
        }

        public void InitiateHeist(ChatMessage message)
        {
            using var context = ContextFactory.GetContext();

            var player = context.GetPlayer(Client, message);

            Client.SpoolMessageAsMe(message.Channel, player,
                "As you approach the great cheese dragon's lair, a mysterious voice begins echoing in your head. " +
                "You can't understand what it is saying, but it fills you with terror. " +
                $"You quiver in fear, and turn back immediately. {Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Negative))}",
                Priority.Low);

            return;

            // Find if there's another ongoing heist in this channel to join.
            if (OngoingHeists.TryGetValue(message.Channel, out IHeist currentHeist))
            {
                JoinHeist(message, player, context);
                return;
            }

            // Trying to initiate new heist
            var now  = DateTime.Now;

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
                    $"A heist has been initiated. " +
                    $"You and others can join with \"!cheese heist <cheese amount>\". " +
                    $"The more cheese wagered, the greater the risk and reward! " +
                    $"The heist will begin in {HeistWaitTime.Format()}.");

                Task.Run(() => JoinHeist(message, player, context));

                // Since we are sleeping, this needs to be async.
                SpinWait.Sleep(HeistWaitTime);

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
                $"You must wait {timeToWait} until you can initiate another heist. {Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Waiting))}");
        }

        private void JoinHeist(ChatMessage message, Player player, IApplicationContext context)
        {
            if (!OngoingHeists.TryGetValue(message.Channel, out IHeist heist))
            {
                Client.SpoolMessageAsMe(message.Channel, player,
                    "There currently is no heist to join in this channel.");
                return;
            }

            String strippedCommand = message.Message.GetNextWord(out _).GetNextWord(out _);

            if (String.IsNullOrWhiteSpace(strippedCommand))
            {
                Client.SpoolMessageAsMe(message.Channel, player,
                    "To join the heist, you must wager some amount of cheese \"!cheese heist <cheese to wager>\". " +
                    "The more you wager, the greater the risk and reward.");
                return;
            }

            strippedCommand.GetNextWord(out String proposedWager);

            if (proposedWager.TryGetWager(out var wager))
            {
                heist.UpdateWager(context, player, wager);
                return;
            }

            Client.SpoolMessageAsMe(message.Channel, player, $"Cannot wager \"{proposedWager}\". You must wager a positive number of cheese to join the heist.");
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
}
