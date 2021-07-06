using System;
using Chubberino.Modules.CheeseGame.Hazards;
using Chubberino.Modules.CheeseGame.Ranks;
using Xunit.Abstractions;

namespace Chubberino.UnitTests.Manual.Modules.CheeseGame.Hazards
{
    public class InfestationCalculation
    {
        public ITestOutputHelper Output { get; }

        public InfestationCalculation(ITestOutputHelper output)
        {
            Output = output;
        }

        // Only for comparing calculation models
        // [Fact]
#pragma warning disable IDE0051 // Remove unused private members
        private void Test()
#pragma warning restore IDE0051 // Remove unused private members
        {
            DisplayInfestValues();
        }

        private void DisplayInfestValues()
        {
            foreach (Rank rank in Enum.GetValues<Rank>())
            {
                if (rank == Rank.None) { continue; }

                var oldV = OldInfest(rank);
                var newV = NewInfest(rank);

                Output.WriteLine(rank.ToString());
                Output.WriteLine(oldV.ToString());
                Output.WriteLine(newV.ToString());
            }
        }

        public static (Double Minimum, Double Average, Double Maximum) OldInfest(Rank rank)
        {
            Int32 minCount = rank > Rank.Bronze ? 1 : 0;
            Int32 maxCount = (Int32)rank + 1;
            Double averageCount = (maxCount + minCount) / 2.0;

            Double chance = (Double)rank / 100;

            return (
                chance * minCount,
                chance * averageCount,
                chance * maxCount);
        }

        public static (Double Minimum, Double Average, Double Maximum) NewInfest(Rank rank)
        {
            Int32 minCount = rank > Rank.Bronze ? 1 : 0;
            Int32 maxCount = HazardManager.InfestationMaximums[rank];
            Double averageCount = (maxCount + minCount) / 2.0;

            Double chance = HazardManager.InfestationChance[rank];

            return (
                chance * minCount,
                chance * averageCount,
                chance * maxCount);
        }
    }
}
