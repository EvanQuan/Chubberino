using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Models;
using System;

namespace Chubberino.Modules.CheeseGame.Bosses
{
    public sealed class BossManager : AbstractCommandStrategy
    {
        public const Int32 BaseCatDamage = 25;

        public BossManager(
            IApplicationContext context,
            ITwitchClientManager twitchClientManager,
            Random random,
            IEmoteManager emoteManager)
            : base(context, twitchClientManager, random, emoteManager)
        {
        }

        /// <summary>
        /// Updates the state of the boss fight. To be called at an interval.
        /// </summary>
        public void OnUpdateBossFight()
        {

            foreach (Player player in Context.Players)
            {
                Int32 playerDamage = player.CatCount * BaseCatDamage;
            }
        }

    }
}
