using Chubberino.Client;
using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public sealed class HeistManager : AbstractCommandStrategy, IHeistManager
    {
        private static TimeSpan HeistWaitTime { get; } = TimeSpan.FromSeconds(30);

        private IList<IHeist> HeistTypes { get; }

        /// <summary>
        /// Key: channel that the heist was initiated in.
        /// Value: The ongoing heist.
        /// </summary>
        private ConcurrentDictionary<String, IHeist> OngoingHeists { get; }

        public static TimeSpan HeistCooldown { get; set; } = TimeSpan.FromHours(20);
        public ISpinWait SpinWait { get; }

        public HeistManager(
            IApplicationContext context,
            ITwitchClientManager client,
            Random random,
            IEmoteManager emoteManager,
            ISpinWait spinWait)
            : base(context, client, random, emoteManager)
        {
            HeistTypes = new List<IHeist>();
            OngoingHeists = new ConcurrentDictionary<String, IHeist>();
            SpinWait = spinWait;
        }

        public IHeistManager AddHeist(IHeist heist)
        {
            HeistTypes.Add(heist);

            return this;
        }

        public void InitiateHeist(ChatMessage message)
        {
            var player = GetPlayer(message);
            var now  = DateTime.Now;

            var timeSinceLastHeistInitiated = now - player.LastHeistInitiated;

            if (timeSinceLastHeistInitiated >= HeistCooldown)
            {
                // Find if there's another ongoing heist in this channel.
                if (OngoingHeists.TryGetValue(message.Channel, out IHeist heist))
                {
                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"There already is an ongoing quest in this channel, initiated by {heist.InitiatorName}. You must wait until it is over before you initiate your own heist in this channel.");
                }
                else if (player.Points < 1)
                {
                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"You do not have any cheese to wager to initiate a heist.");
                }
                else if (OngoingHeists.TryAdd(message.Channel, new Heist(message, Context, Random, TwitchClientManager)))
                {
                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"A heist has been initiated. You and others can join with \"!cheese join <cheese amount>\". The more cheese wagered, the greater the risk and reward! The heist will begin in {Format(HeistWaitTime)}.");

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
                    TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"You have already initiated an ongoing heist.");
                }

            }
            else
            {
                var timeUntilNextHeistAvailable = HeistCooldown - timeSinceLastHeistInitiated;

                var timeToWait = Format(timeUntilNextHeistAvailable);

                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"You must wait {timeToWait} until you can initiate another heist.");
            }

        }

        public void JoinHeist(ChatMessage message)
        {
            var player = GetPlayer(message);

            if (!OngoingHeists.TryGetValue(message.Channel, out IHeist heist))
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, "There currently is no heist to join in this channel");
                return;
            }

            var strippedCommand = message.Message
                .StripStart("!cheese h")
                .StripStart("eist");

            if (!strippedCommand.StartsWith(' '))
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
