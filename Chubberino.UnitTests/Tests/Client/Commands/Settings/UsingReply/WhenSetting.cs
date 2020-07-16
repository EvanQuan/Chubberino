﻿using Chubberino.Client.Commands.Settings;
using System;
using Xunit;

namespace Chubberino.UnitTests.Tests.Client.Commands.Settings.UsingReply
{
    public sealed class WhenSetting : CommandTestBase
    {
        private Repeat Sut { get; }

        public WhenSetting()
        {
            Sut = new Repeat(TwitchClient.Object, Spooler.Object);
        }

        [Theory]
        [InlineData("i", "2.5", 2.5)]
        [InlineData("interval", "1.5", 1.5)]
        [InlineData("interval", "0", 0)]
        [InlineData("i", "-1", 0)]
        [InlineData("interval", "-1.25", 0)]
        public void ShouldSetInterval(String property, String value, Double expectedIntervalSeconds)
        {
            Sut.Set(property, new String[] { value });

            Assert.Equal(expectedIntervalSeconds, Spooler.Object.Interval.TotalSeconds);
        }
    }
}
