using Chubberino.Modules.CheeseGame.Points;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.CheeseModifierManagers
{
    public abstract class UsingCheeseModifierManager
    {
        protected CheeseModifierRepository Sut { get; }

        protected UsingCheeseModifierManager()
        {
            Sut = new CheeseModifierRepository();
        }
    }
}
