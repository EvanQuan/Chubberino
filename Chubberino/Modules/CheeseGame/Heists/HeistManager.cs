using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
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

        public static TimeSpan HeistCooldown { get; set; } = TimeSpan.FromHours(6);

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
