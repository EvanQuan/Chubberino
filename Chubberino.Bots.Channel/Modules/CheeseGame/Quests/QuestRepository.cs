using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Ranks;
using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Quests
{
    public sealed class QuestRepository : IQuestRepository
    {
        public static IReadOnlyList<Quest> Common { get; } = new List<Quest>()
        {
            new GainCheeseQuest(
                "Fontiago Forest",
                "You quickly get lost within the woods. By nightfall, you finally find your way out empty-handed.",
                "You find a hidden cache. Inside is an impressive assortment of cheeses.",
                15,
                Rank.Bronze,
                0.15),
            new GainCheeseQuest(
                "River Ragstone",
                "You sit at the river for hours without anything appearing.",
                "You find some cheese floating down the stream and grab it before it gets away.",
                22,
                Rank.Bronze,
                0.25),
            new GainCheeseQuest(
                "Lake Laguiole",
                "With the bad weather, you can't find any fish.",
                "You go fishing and catch some Taleggio Tuna.",
                28,
                Rank.Bronze,
                0.35),

            new GainCheeseQuest(
                "Weichkaese Woods",
                "You get scared by the spooky noises, and you turn back.",
                "You find a haunted mansion secluded in the maze of trees. Inside is some strange floating cheese, which you take.",
                35,
                Rank.Silver,
                0.15),
            new GainCheeseQuest(
                "Caciotta Cliff",
                "The heights get to you, and you go back without finding anything.",
                "You find some cheese along the edge of the cliffside, which you carefully take.",
                41,
                Rank.Silver,
                0.25),
            new GainCheeseAndStorageQuest(
                "Valencay Valley",
                "The valley winds on forever, and you return without anything.",
                "You find a small cave in the side of the valley, containing a treasure trove of cheese. You claim the cave to store cheese.",
                28,
                (Int32)(Storage.BaseQuantity * 0.1),
                Rank.Silver,
                0.35),

            new GainCheeseQuest(
                "Mount Magna",
                "You search the cavern depths, but with no luck.",
                "You find a giant vein of Magna cheese and mine at it for hours.",
                48,
                Rank.Gold,
                0.15),
            new GainCheeseQuest(
                "Madrona Marsh",
                "You get lost in the fog, and with some trouble, return safely.",
                "You find some cheese hidden in the depths that must have been aging for decades.",
                54,
                Rank.Gold,
                0.25),
            new GainCheeseQuest(
                "Tavoliere Tundra",
                "A blizzard keeps you from exploring very far.",
                "You find cheese frozen in blocks of ice in a cave.",
                60,
                Rank.Gold,
                0.35),


            new GainCheeseQuest(
                "Jarlsberg Jungle",
                "Surrounded by vines and mosquitos, you return without much luck.",
                "Up in the treetops you find some rather exotic cheeses.",
                73,
                Rank.Platinum,
                0.15),
            new GainCheeseQuest(
                "Sancerre Savannah",
                "The lands stretch out into the horizon without anything in sight.",
                "You find some melted cheese basking in the sun's light.",
                85,
                Rank.Platinum,
                0.25),
            new GainCheeseAndStorageQuest(
                "Hayloft Hills",
                "You quickly tire and turn back.",
                "An abandoned hut sits atop a hill with some cheese lying about, which you claim to for storage.",
                60,
                (Int32)(Storage.BaseQuantity * 0.1),
                Rank.Platinum,
                0.35),

            new GainCheeseQuest(
                "the Operetta Ocean",
                "You don't have the equipment needed to traverse it and turn back.",
                "Boarding your submarine, you find a colony of cheese coral at the bottom of the ocean floor and take what you can find.",
                98,
                Rank.Diamond,
                0.15),
            new GainCheeseQuest(
                "Tegan Taiga",
                "The deep powder snow discourages you from travelling too far.",
                "Hiding under the snow-covered branches of a particular pine tree is a small pile of cheese, untouched by the harsh weather.",
                110,
                Rank.Diamond,
                0.25),
            new GainCheeseQuest(
                "Bad Axe Beach",
                "While the view is beautiful, there is nothing to be found.",
                "You uncover a treasure chest of cheese hidden under the sands.",
                122,
                Rank.Diamond,
                0.35),

            new GainCheeseQuest(
                "Galbani Grotto",
                "The ground is slippery and the lighting dark. You turn back before you get hurt.",
                "Baskets of cheese lie in a dimmly-lit alcove.",
                135,
                Rank.Master,
                0.15),
            new GainCheeseQuest(
                "Cotswold Canyon",
                "The steep cliffsides tower over you, bringing a foreboding presence that makes you turn back.",
                "Within the strata of the cliffsides, you find some ancient cheese of days long past.",
                147,
                Rank.Master,
                0.25),
            new GainCheeseAndStorageQuest(
                "Caboc Cave",
                "The place is dark and the ground slippery. You go back immediately.",
                "Traversing through the winding tunnels, you find stalagtites of cheese, which you knock down with your pickaxe. Certain alcoves make good spots for storage.",
                122,
                (Int32)(Storage.BaseQuantity * 0.1),
                Rank.Master,
                0.35),

            new GainCheeseQuest(
                "Vigneron Volcano",
                "You retreat to safety as the the volcano erupts.",
                "Amongst the lava is a pool of melted cheese.",
                166,
                Rank.Grandmaster,
                0.15),
            new GainCheeseQuest(
                "the Fields of Feta",
                "A farmer spots you and threatens you to turn back.",
                "You quickly stuff your pockets before anyone sees you.",
                184,
                Rank.Grandmaster,
                0.25),
            new GainCheeseQuest(
                "the Berkswell Badlands",
                "The scorching heat quickly overwhelms you.",
                "You reach the cheese hoodoos and gather all you can carry.",
                202,
                Rank.Grandmaster,
                0.35),

            new GainCheeseQuest(
                "the Landaff Labrynth",
                "You get lost in the maze and take far too much time to return back from where you came.",
                "At the heart of the maze, you find a giant mound of cheese for the taking.",
                221,
                Rank.Legend,
                0.15),
            new GainCheeseAndStorageQuest(
                "I' Blu Isle",
                "The terrain is disorienting and the vegetation is too overgrown to let you travel very deep.",
                "You find cheese all over the place and take the effort to store some of it on the island in hidden caches.",
                202,
                (Int32)(Storage.BaseQuantity * 0.1),
                Rank.Legend,
                0.25),
            new GainCheeseQuest(
                "the Great Cheese Emporium",
                "The place is too well-guarded and you fail to pass the outer gate.",
                "You sneak into the facility and find a mountain of cheese, which you take through a secret underground exit.",
                600,
                Rank.Legend,
                0.35),

        };

        public static IReadOnlyList<Quest> Rare { get; } = new List<Quest>()
        {
            // new GainCatQuest(),
            new GainStorageQuest(
                "Cantal Chaparral",
                "The land is vast, but the ground is rife with vegetation.",
                "You find an open clearing perfect to setup a warehouse for cheese storage.",
                (Int32)(Storage.BaseQuantity * 1.5)),
            new GainWorkerQuest(
                Rank.Bronze,
                0)
        };

        public IReadOnlyList<Quest> CommonQuests { get; } = Common;

        public IReadOnlyList<Quest> RareQuests { get; } = Rare;
    }
}
