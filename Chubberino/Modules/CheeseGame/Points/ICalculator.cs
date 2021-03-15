using Chubberino.Modules.CheeseGame.Rankings;
using System;

namespace Chubberino.Modules.CheeseGame.Points
{
    public interface ICalculator
    {
        Double GetQuestRewardMultiplier(Rank rank);
    }
}
