using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class GainCatQuest : Quest
    {
        public GainCatQuest()
            : base("Gornoaltajski Grasslands",
                  "You search around the field, seeing nothing but the grass blowing in the wind.",
                  (player, emote) =>
                  {
                      player.CatCount++;
                      return "You see a stray cat frollicking about, and decide to give it a new home. " +
                      $"{emote} (+1 cat)";
                  },
                  _ => "+1 cat",
                  Rank.Bronze,
                  0)
        {
        }
    }
}
