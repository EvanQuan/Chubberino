using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class GainWorkerQuest : Quest
    {
        public GainWorkerQuest(Rank rankToUnlock, Double rankPricePercentPrice)
            : base("Panela Plains",
                   "You search around and don't find anything interesting. You return to your cheese factory empty-handed.",
                  (player, emote) =>
                  {
                      if (player.WorkerCount + 1 > player.PopulationCount)
                      {
                          Int32 rewardPoints = player.GetWorkerPrice();
                          player.AddPoints(rewardPoints);

                          return "You come across a lonely traveller looking for work. " +
                          "Unfortunately, you do not have enough population slots for him to join your efforts, and so gives you some cheese instead. " +
                          $"{emote} (+{rewardPoints} cheese)";
                      }
                      else
                      {
                          player.WorkerCount++;
                          return "You come across a lonely traveller looking for work. " +
                          "He decides to join your efforts. " +
                          $"{emote} (+1 worker)";
                      }
                  },
                  _ => "+1 worker",
                  rankToUnlock,
                  rankPricePercentPrice)
        {
        }
    }
}
