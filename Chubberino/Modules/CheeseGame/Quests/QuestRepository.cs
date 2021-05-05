using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.Rankings;
using Chubberino.Utility;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class QuestRepository : IRepository<Quest>
    {
        /// <summary>
        /// Chance that a quest from the rare quest pool will be chosen.
        /// </summary>
        public const Double RareQuestChance = 0.01;

        public static IReadOnlyList<Quest> Quests { get; } = new List<Quest>()
        {
            new GainCheeseQuest(
                "Fontiago Forest",
                "You quickly get lost within the woods. By nightfall, you finally find your way out empty-handed.",
                "You find a hidden cache. Inside is an impressive assortment of cheeses.",
                10,
                Rank.Bronze,
                0.15),
            new GainCheeseQuest(
                "River Ragstone",
                "You sit at the river for hours without anything appearing.",
                "You find some cheese floating down the stream and grab it before it gets away.",
                15,
                Rank.Bronze,
                0.25),
            new GainCheeseQuest(
                "Lake Laguiole",
                "With the bad weather, you can't find any fish.",
                "You go fishing and catch some Taleggio Tuna.",
                20,
                Rank.Bronze,
                0.35),

            new GainCheeseQuest(
                "Weichkaese Woods",
                "You get scared by the spooky noises, and you turn back.",
                "You find a haunted mansion secluded in the maze of trees. Inside is some strange floating cheese, which you take.",
                25,
                Rank.Silver,
                0.15),
            new GainCheeseQuest(
                "Caciotta Cliff",
                "The heights get to you, and you go back without finding anything.",
                "You find some cheese along the edge of the cliffside, which you carefully take.",
                30,
                Rank.Silver,
                0.25),
            new GainCheeseAndStorageQuest(
                "Valencay Valley",
                "The valley winds on forever, and you return without anything.",
                "You find a small cave in the side of the valley, containing a treasure trove of cheese. You claim the cave to store cheese.",
                20,
                (Int32)(Constants.ShopStorageQuantity * 0.1),
                Rank.Silver,
                0.35),

            new GainCheeseQuest(
                "Mount Magna",
                "You search the cavern depths, but with no luck.",
                "You find a giant vein of Magna cheese and mine at it for hours.",
                35,
                Rank.Gold,
                0.15),
            new GainCheeseQuest(
                "Madrona Marsh",
                "You get lost in the fog, and with some trouble, return safely.",
                "You find some cheese hidden in the depths that must have been aging for decades.",
                40,
                Rank.Gold,
                0.25),
            new GainCheeseQuest(
                "Tavoliere Tundra",
                "A blizzard keeps you from exploring very far.",
                "You find cheese frozen in blocks of ice in a cave.",
                45,
                Rank.Gold,
                0.35),


            new GainCheeseQuest(
                "Jarlsberg Jungle",
                "Surrounded by vines and mosquitos, you return without much luck.",
                "Up in the treetops you find some rather exotic cheeses.",
                55,
                Rank.Platinum,
                0.15),
            new GainCheeseQuest(
                "Sancerre Savannah",
                "The lands stretch out into the horizon without anything in sight.",
                "You find some melted cheese basking in the sun's light.",
                65,
                Rank.Platinum,
                0.25),
            new GainCheeseAndStorageQuest(
                "Hayloft Hills",
                "You quickly tire and turn back.",
                "An abandoned hut sits atop a hill with some cheese lying about, which you claim to for storage.",
                45,
                (Int32)(Constants.ShopStorageQuantity * 0.1),
                Rank.Platinum,
                0.35),

            new GainCheeseQuest(
                "the Operetta Ocean",
                "You don't have the equipment needed to traverse it and turn back.",
                "Boarding your submarine, you find a colony of cheese coral at the bottom of the ocean floor and take what you can find.",
                75,
                Rank.Diamond,
                0.15),
            new GainCheeseQuest(
                "Tegan Taiga",
                "The deep powder snow discourages you from travelling too far.",
                "Hiding under the snow-covered branches of a particular pine tree is a small pile of cheese, untouched by the harsh weather.",
                85,
                Rank.Diamond,
                0.25),
            new GainCheeseQuest(
                "Bad Axe Beach",
                "While the view is beautful, there is nothing to be found.",
                "You uncover a treasure chest of cheese hidden under the sands.",
                95,
                Rank.Diamond,
                0.35),

            new GainCheeseQuest(
                "Galbani Grotto",
                "The ground is slippery and the lighting dark. You turn back before you get hurt.",
                "Baskets of cheese lie in a dimmly-lit alcove.",
                105,
                Rank.Master,
                0.15),
            new GainCheeseQuest(
                "Cotswold Canyon",
                "The steep cliffsides tower over you, bringing a foreboding presence that makes you turn back.",
                "Within the strata of the cliffsides, you find some ancient cheese of days long past.",
                115,
                Rank.Master,
                0.25),
            new GainCheeseAndStorageQuest(
                "Caboc Cave",
                "The place is dark and the ground slippery. You go back immediately.",
                "Traversing through the winding tunnels, you find stalagmites of cheese, which you knock down with your pickaxe. Certain alcoves make good spots for storage.",
                95,
                (Int32)(Constants.ShopStorageQuantity * 0.1),
                Rank.Master,
                0.35),

            new GainCheeseQuest(
                "Vigneron Volcano",
                "You retreat to safety as the the volcano erupts.",
                "Amongst the lava is a pool of melted cheese.",
                130,
                Rank.Grandmaster,
                0.15),
            new GainCheeseQuest(
                "the Fields of Feta",
                "A farmer spots you and threatens you to turn back.",
                "You quickly stuff your pockets before anyone sees you.",
                145,
                Rank.Grandmaster,
                0.25),
            new GainCheeseQuest(
                "the Berkswell Badlands",
                "The scorching heat quickly overwhelms you.",
                "You reach the cheese hoodoos and gather all you can carry.",
                160,
                Rank.Grandmaster,
                0.35),

            new GainCheeseQuest(
                "the Landaff Labrynth",
                "You get lost in the maze and take far too much time to return back from where you came.",
                "At the heart of the maze, you find a giant mound of cheese for the taking.",
                175,
                Rank.Legend,
                0.15),
            new GainCheeseAndStorageQuest(
                "I' Blu Isle",
                "The terrain is disorienting and the vegetation is too overgrown to let you travel very deep.",
                "You find cheese all over the place and take the effort to store some of it on the island in hidden caches.",
                160,
                (Int32)(Constants.ShopStorageQuantity * 0.1),
                Rank.Legend,
                0.25),
            new GainWorkerQuest(
                Rank.Legend,
                0.35)

        };

        public static IReadOnlyList<Quest> RareQuests { get; } = new List<Quest>()
        {
            new GainCatQuest(),
            new GainStorageQuest(
                "Cantal Chaparral",
                "",
                "",
                Constants.ShopStorageQuantity),
            new GainWorkerQuest(
                Rank.Bronze,
                0)
        };

        public Random Random { get; }

        public QuestRepository(Random random)
        {
            Random = random;
        }

        public Quest GetRandom(Int32 unlocked)
        {
            return Quests[Random.Next((unlocked - 1).Min(Quests.Count - 1).Max(0))];
        }

        public Boolean TryGetNextToUnlock(Player player, out Quest nextUnlock)
        {
            Int32 nextQuest = player.QuestsUnlockedCount;
            if (nextQuest >= Quests.Count)
            {
                nextUnlock = default;
                return false;
            }

            nextUnlock = Quests[nextQuest];
            return true;
        }
    }
}
