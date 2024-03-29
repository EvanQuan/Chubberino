﻿using Chubberino.Bots.Channel.Modules.CheeseGame.Emotes;
using Chubberino.Database.Contexts;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Emotes;

public abstract class UsingEmoteManager
{
    protected Mock<IApplicationContextFactory> MockedContextFactory { get; }

    protected Mock<IApplicationContext> MockedContext { get; }

    protected Mock<Random> MockedRandom { get; }

    protected EmoteManager Sut { get; }

    protected UsingEmoteManager()
    {
        MockedContextFactory = new();
        MockedContext = new();
        MockedRandom = new();

        Sut = new(MockedContextFactory.Object, MockedRandom.Object);

        MockedContextFactory
            .Setup(x => x.GetContext())
            .Returns(MockedContext.Object);
    }
}
