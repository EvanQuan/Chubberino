using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Utility;
using System;
using System.Collections.Concurrent;
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

        public static TimeSpan HeistCooldown { get; set; } = TimeSpan.FromHours(10);
        public ISpinWait SpinWait { get; }

        public HeistManager(
            IApplicationContext context,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager,
            ISpinWait spinWait)
            : base(context, client, random, emoteManager)
        {
            OngoingHeists = new ConcurrentDictionary<String, IHeist>();
            SpinWait = spinWait;
        }

        public void InitiateHeist(ChatMessage message)
        {
            var player = GetPlayer(message);
            var now  = DateTime.Now;

            var timeSinceLastHeistInitiated = now - player.LastHeistInitiated;

            if (timeSinceLastHeistInitiated >= HeistCooldown)
            {
                // Find if there's another ongoing heist in this channel.
                if (OngoingHeists.TryGetValue(message.Channel, out IHeist currentHeist))
                {
                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"There already is an ongoing heist in this channel, initiated by {currentHeist.InitiatorName}. You must wait until it is over before you initiate your own heist in this channel.");
                }
                else if (player.Points < 1)
                {
                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"You do not have any cheese to wager to initiate a heist.");
                }
                else 
                {
                    var heist = new Heist(message, Context, Random, TwitchClientManager);
                    OngoingHeists.TryAdd(message.Channel, heist);

                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"A heist has been initiated. You and others can join with \"!cheese join <cheese amount>\". The more cheese wagered, the greater the risk and reward! The heist will begin in {HeistWaitTime.Format()}.");

                    Task.Run(() => JoinHeist(message));

                    // Since we are sleeping, this needs to be async.
                    SpinWait.Sleep(HeistWaitTime);

                    if (heist.Start())
                    {
                        player.LastHeistInitiated = now;

                        Context.SaveChanges();
                    }

                    OngoingHeists.TryRemove(message.Channel, out _);
                }
            }
            else
            {
                var timeUntilNextHeistAvailable = HeistCooldown - timeSinceLastHeistInitiated;

                var timeToWait = timeUntilNextHeistAvailable.Format();

                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"You must wait {timeToWait} until you can initiate another heist.");
            }

        }

        public void JoinHeist(ChatMessage message)
        {
            var player = GetPlayer(message);

            if (!OngoingHeists.TryGetValue(message.Channel, out IHeist heist))
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, "There currently is no heist to join in this channel.");
                return;
            }

            String strippedCommand = message.Message.GetNextWord(out _).GetNextWord(out _);

            if (String.IsNullOrWhiteSpace(strippedCommand))
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, "To join the heist, you must wager some amount of cheese \"!cheese join <cheese to wager>\". The more you wager, the greater the risk and reward.");
                return;
            }

            strippedCommand.GetNextWord(out String countUnchecked);

            if (Int32.TryParse(countUnchecked, out Int32 wager))
            {
                heist.UpdateWager(player, wager);
            }
            else if (countUnchecked.Equals("all", StringComparison.OrdinalIgnoreCase) || countUnchecked.Equals("a", StringComparison.OrdinalIgnoreCase))
            {
                heist.UpdateWager(player, player.Points);
            }
            else
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"\"{countUnchecked}\" You must wager a positive number of cheese to join the heist.");
            }
        }
    }
}
