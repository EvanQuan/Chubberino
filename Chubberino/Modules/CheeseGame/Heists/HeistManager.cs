using Chubberino.Client.Abstractions;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

        public void StartHeist(ChatMessage message)
        {
            var player = GetPlayer(message);
            var now  = DateTime.Now;

            var timeSinceLastHeistInitiated = now - player.LastHeistInitiated;

            if (timeSinceLastHeistInitiated >= HeistCooldown)
            {
                var heist = Random.GetElement(HeistTypes);

                // Find if there's another ongoing heist in this channel.
                // if (OngoingHeists.TryGetValue(message.Channel, out IHeist heist))
                {
                    TwitchClientManager.Client.SpoolMessage(message.Channel, $"{player.GetDisplayName()} There already is an ongoing quest in this channel, initiated by {heist.InitatorName}. You must wait until it is over before you initiate your own heist in this channel.");
                }
                // else if (OngoingHeists.TryAdd(message.Channel, player.Name))
                {
                    TwitchClientManager.Client.SpoolMessage(message.Channel, $"{player.GetDisplayName()} A heist has been initiated. Others can join with \"!cheese join <cheese amount>\". The more cheese invested, the greater the risk and reward! The heist will begin in {Format(HeistWaitTime)}.");

                    SpinWait.Sleep(HeistWaitTime);

                    heist.Start(message, player);

                    player.LastHeistInitiated = now;


                    Context.SaveChanges();
                }
                // else
                {
                    TwitchClientManager.Client.SpoolMessage(message.Channel, $"{player.GetDisplayName()} You have already initiated an ongoing heist.");
                }

            }
            else
            {
                var timeUntilNextHeistAvailable = HeistCooldown - timeSinceLastHeistInitiated;

                var timeToWait = Format(timeUntilNextHeistAvailable);

                TwitchClientManager.Client.SpoolMessage(message.Channel, $"{player.GetDisplayName()}, you must wait {timeToWait} until you can initiate another heist.");
            }

        }
    }
}
