using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Points;
using Moq;
using System;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Points.CheeseRepositories
{
    public abstract class UsingCheeseRepository
    {
        protected CheeseRepository Sut { get; }

        protected Mock<Random> MockedRandom { get; }

        protected Player Player { get; }

        protected UsingCheeseRepository()
        {
            MockedRandom = new Mock<Random>();

            Sut = new CheeseRepository(MockedRandom.Object);

            Player = new Player();
        }
    }
}
