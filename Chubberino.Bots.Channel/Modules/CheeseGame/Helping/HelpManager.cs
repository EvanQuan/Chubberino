using System;
using Chubberino.Bots.Channel.Modules.CheeseGame;
using Chubberino.Common.Extensions;
using Chubberino.Database.Contexts;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Items.Workers;
using Chubberino.Modules.CheeseGame.Ranks;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Helping
{
    public sealed class HelpManager : IHelpManager
    {
        public static class Messages
        {
            public const String Generic = 
                    "Commands: !cheese <command> where command is " +
                    "| shop - look at what is available to buy with cheese " +
                    "| buy <item> - buy an item at the shop " +
                    "| help <item> - get information about an item in the shop " +
                    "| quest - potentially get a big reward. The more workers you have, the greater chance of success. " +
                    "| heist - Gamble some cheese, to potentially get more in return. " +
                    "| rank - show information about your rank " +
                    "| rankup - Spend cheese to unlock new items to buy at the shop.";

            public const String Storage = "Storage increases the maximum amount of cheese you can have. There is no limit to gaining more storage.";
            public const String Population = "Population increases the maximum number of workers you can have. You can have as many workers as you have population.";
            public static String Worker { get; } = $"Workers increase the amount of cheese you get every time you gain cheese with \"!cheese\" and when you go on a quest with \"!cheese quest\". " +
                $"Initially they each give an additive {RankWorkerUpgradeExtensions.BaseWorkerPointPercent * 100}% bonus to cheese gains, which can be further increased with certain upgrades. " +
                $"The number of workers you can have is limited by your population.";
            public const String Quest = "Go on a random quest to get rewards with \"!cheese quest\". The chance of success increases with how much gear you have. You can buy gear with \"!cheese buy gear\".";
            public const String Recipe = "Recipes allow you to create new kinds of cheese with \"!cheese\". " +
                "Every recipe you gain is added the pool of possible recipes you could create from \"!cheese\". " +
                "Every recipe has an equal likihood of getting chosen.";
            public static String Rank { get; } = $"Ranks unlock new items to buy at the shop. " +
                $"Eventually ranking will give you prestige, reseting your rank and everything you have to restart the climb. " +
                $"For every prestige you gain, you get a permanent {(Int32)(RankManager.PrestigeBonus * 100)}% boost to your cheese gains, which can stack. " +
                $"Be warned that with every rank up, you increase the chances of getting attacked by a rat infestation.";
            public const String Upgrade = "Upgrades provide a permanent bonus to your cheese factory until you prestige. Each rank unlocks a new set of upgrades you can buy.";
            public static String Gears { get; } = $"Gear provides you with a {Gear.QuestSuccessBonus * 100}% quest success chance for each you have. There is no limit to the number of gear you can have.";
            public const String Mouse = "Mousetraps kills giant rats that infest your cheese factory, allow you to maintain or recover any worker bonuses you have.";
            public const String Cat = "[CURRENTLY DO NOTHING] Cats help you fight against the giant evil mouse, Chubshan the Immortal. The more cats you have, the more you will be rewarded when Chubshan is defeated.";
            public const String Invalid = "Invalid item \"{0}\" name. Type \"!cheese shop\" to see the items available for purchase.";
        }

        public IApplicationContextFactory ContextFactory { get; }

        public ITwitchClientManager Client { get; }

        public HelpManager(
            IApplicationContextFactory contextFactory,
            ITwitchClientManager client)
        {
            ContextFactory = contextFactory;
            Client = client;
        }

        public void GetHelp(ChatMessage chatMessage)
        {
            // Cut out "!cheese help" start.
            chatMessage.Message
                .GetNextWord(out _)
                .GetNextWord(out _)
                .GetNextWord(out var item);

            using var context = ContextFactory.GetContext();

            var player = context.GetPlayer(Client, chatMessage);

            var helpMessage = GetHelpMessage(item);

            Client.SpoolMessageAsMe(chatMessage.Channel, player, helpMessage, Priority.Low);
        }

        private static String GetHelpMessage(String item)
        {
            if (String.IsNullOrWhiteSpace(item))
            {
                return Messages.Generic;
            }

            return item.ToLower() switch
            {
                "s" or "storage" => Messages.Storage,
                "p" or "population" => Messages.Population,
                "w" or "worker" or "workers" => Messages.Worker,
                "q" or "quest" or "quests" => Messages.Quest,
                "recipe" or "recipes" => Messages.Recipe,
                "r" or "rank" or "ranks" or "bronze" or "silver" or "gold" or "diamond" or "platinum" or "master" or "grandmaster" or "legend" => Messages.Rank,
                "u" or "upgrade" or "upgrades" => Messages.Upgrade,
                "g" or "gear" => Messages.Gears,
                "m" or "mouse" or "mousetrap" or "mousetraps" => Messages.Mouse,
                "c" or "cat" or "cats" => Messages.Cat,
                _ => Messages.Invalid.Format(item)
            };
        }
    }
}
