using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Client.Services;
using Chubberino.Database.Contexts;
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
    public sealed class HeistManager : AbstractCommandStrategy, IHeistManager
    {
        private static TimeSpan HeistWaitTime { get; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Key: channel that the heist was initiated in.
        /// Value: The ongoing heist.
        /// </summary>
        private ConcurrentDictionary<String, IHeist> OngoingHeists { get; }

        public static TimeSpan HeistCooldown { get; set; } = TimeSpan.FromHours(6);

        public ISpinWait SpinWait { get; }
        public IDateTimeService DateTime { get; }

        public HeistManager(
            IApplicationContext context,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager,
            ISpinWait spinWait,
            IDateTimeService dateTime)
            : base(context, client, random, emoteManager)
        {
            OngoingHeists = new ConcurrentDictionary<String, IHeist>();
            SpinWait = spinWait;
            DateTime = dateTime;
        }

        public void InitiateHeist(ChatMessage message)
        {
            var player = GetPlayer(message);

            // Find if there's another ongoing heist in this channel to join.
            if (OngoingHeists.TryGetValue(message.Channel, out IHeist currentHeist))
            {
                JoinHeist(message, player);
            }
            else 
            {
                // Trying to initiate new heist
                var now  = DateTime.Now;

                var timeSinceLastHeistInitiated = now - player.LastHeistInitiated;

                if (timeSinceLastHeistInitiated >= HeistCooldown)
                {
                    var heist = new Heist(message, Context, Random, TwitchClientManager);
                    OngoingHeists.TryAdd(message.Channel, heist);

                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player,
                        $"A heist has been initiated. " +
                        $"You and others can join with \"!cheese heist <cheese amount>\". " +
                        $"The more cheese wagered, the greater the risk and reward! " +
                        $"The heist will begin in {HeistWaitTime.Format()}.");

                    Task.Run(() => JoinHeist(message, player));

                    // Since we are sleeping, this needs to be async.
                    SpinWait.Sleep(HeistWaitTime);

                    if (heist.Start())
                    {
                        player.LastHeistInitiated = now;

                        Context.SaveChanges();
                    }

                    OngoingHeists.TryRemove(message.Channel, out _);
                }
                else
                {
                    var timeUntilNextHeistAvailable = HeistCooldown - timeSinceLastHeistInitiated;

                    var timeToWait = timeUntilNextHeistAvailable.Format();

                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player,
                        $"You must wait {timeToWait} until you can initiate another heist.");
                }
            }
        }

        private void JoinHeist(ChatMessage message, Player player)
        {
            if (!OngoingHeists.TryGetValue(message.Channel, out IHeist heist))
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, player,
                    "There currently is no heist to join in this channel.");
                return;
            }

            String strippedCommand = message.Message.GetNextWord(out _).GetNextWord(out _);

            if (String.IsNullOrWhiteSpace(strippedCommand))
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, player,
                    "To join the heist, you must wager some amount of cheese \"!cheese heist <cheese to wager>\". " +
                    "The more you wager, the greater the risk and reward.");
                return;
            }

            strippedCommand.GetNextWord(out String proposedWager);

            if (player.TryGetWager(proposedWager, out Int32 wager))
            {
                heist.UpdateWager(player, wager);
            }
            else
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"Cannot wager \"{proposedWager}\". You must wager a positive number of cheese to join the heist.");
            }
        }

        public void LeaveAllHeists(Player player)
        {
            IEnumerable<IHeist> heists = OngoingHeists
                .Where(x => x.Value.Wagers.Any(x => x.PlayerTwitchID == player.TwitchUserID))
                .Select(x => x.Value);

            foreach (var heist in heists)
            {
                heist.UpdateWager(player, 0, silent: true);
            }
        }
    }
}
