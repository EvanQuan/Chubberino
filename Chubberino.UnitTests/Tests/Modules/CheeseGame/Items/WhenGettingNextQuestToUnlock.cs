﻿using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Quests;
using System.Collections.Generic;
using Xunit;

namespace Chubberino.UnitTests.Tests.Modules.CheeseGame.Items
{
    public sealed class WhenGettingNextQuestToUnlock
    {
        private QuestLocation Sut { get; }

        private IReadOnlyList<Quest> Repository { get; }

        private Player Player { get; }

        public WhenGettingNextQuestToUnlock()
        {
            Repository = QuestRepository.Quests;

            Player = new Player();

            Sut = new QuestLocation(Repository);
        }

        [Fact]
        public void ShouldReturnFirstQuest()
        {
            var prompt = Sut.GetShopPrompt(Player);

            Assert.Contains(Repository[0].Location, prompt);
        }

        /// <summary>
        /// When there are no more quests to unlock, the prompt should be null.
        /// </summary>
        [Fact]
        public void ShouldReturnNull()
        {
            Player.QuestsUnlockedCount = Repository.Count;

            var prompt = Sut.GetShopPrompt(Player);

            Assert.Null(prompt);
        }

    }
}
