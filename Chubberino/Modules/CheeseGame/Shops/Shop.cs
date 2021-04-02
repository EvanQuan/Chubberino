using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Emotes;
using Chubberino.Modules.CheeseGame.Items;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Modules.CheeseGame.Points;
using Chubberino.Modules.CheeseGame.Quests;
using Chubberino.Modules.CheeseGame.Upgrades;
using Chubberino.Utility;
using System;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Shops
{
    public class Shop : AbstractCommandStrategy, IShop
    {
        public IRepository<CheeseType> CheeseRepository { get; }
        public IRepository<Quest> QuestRepository { get; }
        public IUpgradeManager UpgradeManager { get; }
        public IItemManager ItemManager { get; }

        public Shop(
            IApplicationContext context,
            ITwitchClientManager client,
            IRepository<CheeseType> cheeseRepository,
            IRepository<Quest> questRepository,
            Random random,
            IEmoteManager emoteManager,
            IUpgradeManager upgradeManager,
            IItemManager itemManager)
            : base(context, client, random, emoteManager)
        {
            CheeseRepository = cheeseRepository;
            QuestRepository = questRepository;
            UpgradeManager = upgradeManager;
            ItemManager = itemManager;
        }

        public void ListInventory(ChatMessage message)
        {
            Player player = GetPlayer(message);

            PriceList prices = ItemManager.GetPrices(player);

            String recipePrompt;
            if (CheeseRepository.TryGetNextToUnlock(player, out CheeseType nextCheeseToUnlock))
            {
                if (nextCheeseToUnlock.RankToUnlock > player.Rank)
                {
                    recipePrompt = $"{nextCheeseToUnlock.Name} (+{nextCheeseToUnlock.PointValue})] unlocked at {player.Rank.Next()} rank"; 
                }
                else
                {
                    recipePrompt = $"{nextCheeseToUnlock.Name} (+{nextCheeseToUnlock.PointValue})] for {nextCheeseToUnlock.CostToUnlock} cheese"; 
                }
            }
            else
            {
                recipePrompt = "OUT OF ORDER]";
            }

            String questPrompt;
            if (QuestRepository.TryGetNextToUnlock(player, out Quest nextQuestToUnlock))
            {
                if (nextQuestToUnlock.RankToUnlock > player.Rank)
                {
                    questPrompt = $"{nextQuestToUnlock.Location} ({nextQuestToUnlock.RewardDescription})] unlocked at {player.Rank.Next()} rank"; 
                }
                else
                {
                    questPrompt = $"{nextQuestToUnlock.Location} ({nextQuestToUnlock.RewardDescription})] for {nextQuestToUnlock.Price} cheese"; 
                }
            }
            else
            {
                questPrompt = "OUT OF ORDER]";
            }
            

            String upgradePrompt;
            if (UpgradeManager.TryGetNextUpgradeToUnlock(player, out Upgrade upgrade))
            {
                if (upgrade.RankToUnlock > player.Rank)
                {
                    upgradePrompt = $"{upgrade.Description}] unlocked at {upgrade.RankToUnlock} rank";
                }
                else
                {
                    upgradePrompt = $"{upgrade.Description}] for {upgrade.Price} cheese";
                }
            }
            else
            {
                upgradePrompt = "OUT OF ORDER]";
            }

            Int32 storageGain = (Int32)(Constants.ShopStorageQuantity * player.GetStorageUpgradeMultiplier());

            TwitchClientManager.SpoolMessageAsMe(message.Channel, player,
                $" | Recipe [{recipePrompt}" +
                $" | Storage [+{storageGain}] for {prices.Storage} cheese" +
                $" | Quest [{questPrompt}" +
                $" | Upgrade [{upgradePrompt}" + 
                $" | Worker [+1] for {prices.Worker} cheese" +
                $" | Population [+5] for {prices.Population} cheese" +
                $" | Mousetrap [+1] for {prices.MouseTrap} " +
                "|",
                Priority.Low);
        }

        public void BuyItem(ChatMessage message)
        {
            // Cut out "!cheese buy " or "!cheese b" start.
            String arguments = message.Message
                .GetNextWord(out _)
                .GetNextWord(out _);

            Player player = GetPlayer(message);

            if (String.IsNullOrWhiteSpace(arguments))
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, player, $"Please enter an item to buy with \"!cheese buy <name of item>\". Type \"!cheese shop\" to see the items available for purchase.", Priority.Low);
                return;
            }

            // Cut out space between buy and item
            String remainingArguments = arguments.GetNextWord(out String itemToBuy);

            PriceList prices = ItemManager.GetPrices(player);

            String outputMessage;

            Priority priority = Priority.Low;

            switch (itemToBuy.ToLower()[0])
            {
                case 's':
                    Int32 storageGain = (Int32)(Constants.ShopStorageQuantity * player.GetStorageUpgradeMultiplier());
                    if (player.Points >= prices.Storage)
                    {
                        player.MaximumPointStorage += Constants.ShopStorageQuantity;
                        player.Points -= prices.Storage;
                        Context.SaveChanges();
                        outputMessage = $"You bought {storageGain} storage space. (-{prices.Storage} cheese)";
                        priority = Priority.Medium;
                    }
                    else
                    {
                        outputMessage = $"You need {prices.Storage - player.Points} more cheese to buy {storageGain} storage.";
                    }
                    break;
                case 'p':
                    if (player.Points >= prices.Population)
                    {
                        player.PopulationCount += 5;
                        player.Points -= prices.Population;
                        Context.SaveChanges();
                        outputMessage = $"You bought 5 population slots. (-{prices.Population} cheese)";
                        priority = Priority.Medium;
                    }
                    else
                    {
                        outputMessage = $"You need {prices.Population - player.Points} more cheese to buy 5 population slots.";
                    }
                    break;
                case 'w':
                    if (player.Points >= prices.Worker)
                    {
                        if (player.WorkerCount < player.PopulationCount)
                        {
                            player.WorkerCount += 1;
                            player.Points -= prices.Worker;
                            Context.SaveChanges();
                            outputMessage = $"You bought 1 worker. (-{prices.Worker} cheese)";
                            priority = Priority.Medium;
                        }
                        else
                        {
                            outputMessage = $"You do not have enough population slots for another worker. Consider buying more population slots.";
                        }
                    }
                    else
                    {
                        outputMessage = $"You need {prices.Worker - player.Points} more cheese to buy 1 worker.";
                    }
                    break;
                case 'r':
                    if (CheeseRepository.TryGetNextToUnlock(player, out CheeseType nextCheeseToUnlock))
                    {
                        if (nextCheeseToUnlock.RankToUnlock > player.Rank)
                        {
                            outputMessage = $"You must rankup to {nextCheeseToUnlock.RankToUnlock} rank before you can buy the {nextCheeseToUnlock.Name} recipe.";
                        }
                        else if (player.Points >= nextCheeseToUnlock.CostToUnlock)
                        {
                            player.CheeseUnlocked++;
                            if (nextCheeseToUnlock.UnlocksNegativeCheese)
                            {
                                // Increment again so that the next cheese to unlock is not a negative one.
                                player.CheeseUnlocked++;
                            }
                            player.Points -= nextCheeseToUnlock.CostToUnlock;
                            Context.SaveChanges();
                            outputMessage = $"You bought the {nextCheeseToUnlock.Name} recipe. (-{nextCheeseToUnlock.CostToUnlock} cheese)";
                            priority = Priority.Medium;
                        }
                        else
                        {
                            outputMessage = $"You need {nextCheeseToUnlock.CostToUnlock - player.Points} more cheese to buy the {nextCheeseToUnlock.Name} recipe.";
                        }
                    }
                    else
                    {
                        outputMessage = $"There is no recipe for sale right now.";
                    }
                    break;
                case 'q':
                    if (QuestRepository.TryGetNextToUnlock(player, out Quest nextQuestToUnlock))
                    {
                        if (nextQuestToUnlock.RankToUnlock > player.Rank)
                        {
                            outputMessage = $"You must rankup to {nextQuestToUnlock.RankToUnlock} rank before you can buy a map to {nextQuestToUnlock.Location}.";
                        }
                        else if (player.Points >= nextQuestToUnlock.Price)
                        {
                            player.QuestsUnlockedCount++;
                            player.Points -= nextQuestToUnlock.Price;
                            Context.SaveChanges();
                            outputMessage = $"You bought a map to {nextQuestToUnlock.Location}. (-{nextQuestToUnlock.Price} cheese)";
                            priority = Priority.Medium;
                        }
                        else
                        {
                            outputMessage = $"You need {nextQuestToUnlock.Price - player.Points} more cheese to buy a map to {nextQuestToUnlock.Location}.";
                        }
                    }
                    else
                    {
                        outputMessage = $"There is no quest map for sale right now.";
                    }
                    break;
                case 'u':
                    if (UpgradeManager.TryGetNextUpgradeToUnlock(player, out Upgrade upgrade))
                    {
                        if (upgrade.RankToUnlock > player.Rank)
                        {
                            outputMessage = $"You must rankup to {upgrade.RankToUnlock} rank before you can buy the {upgrade.Description} upgrade.";
                        }
                        else if (player.Points >= upgrade.Price)
                        {
                            upgrade.UpdatePlayer(player);
                            player.Points -= upgrade.Price;
                            Context.SaveChanges();
                            outputMessage = $"You bought the {upgrade.Description} upgrade. (-{upgrade.Price} cheese)";
                            priority = Priority.Medium;
                        }
                        else
                        {
                            outputMessage = $"You need {upgrade.Price - player.Points} more cheese to buy the {upgrade.Description} upgrade.";
                        }
                    }
                    else
                    {
                        outputMessage = $"There is no upgrade for sale right now.";
                    }
                    break;
                case 'm':
                    if (player.Points >= prices.MouseTrap)
                    {
                        remainingArguments.GetNextWord(out String quantityString);

                        Int32 quantityRequested = Int32.TryParse(quantityString, out Int32 result) && result > 0 ? result : 1;

                        Int32 quantityCanAfford = (Int32)Math.Floor(player.Points / (Double)prices.MouseTrap);

                        Int32 quantityToPurchase = Math.Min(quantityRequested, quantityCanAfford);

                        Int32 totalPrice = prices.MouseTrap * quantityToPurchase; 

                        player.MouseTrapCount += quantityToPurchase;
                        player.Points -= totalPrice;
                        Context.SaveChanges();
                        outputMessage = $"You bought {quantityToPurchase} mousetrap{(quantityToPurchase == 1 ? "" : "s")}. (-{totalPrice} cheese)";
                        priority = Priority.Medium;
                    }
                    else
                    {
                        outputMessage = $"You need {prices.MouseTrap - player.Points} more cheese to buy a mousetrap.";
                    }
                    break;
                default:
                    outputMessage = $"Invalid item \"{itemToBuy}\" to buy. Type \"!cheese shop\" to see the items available for purchase.";
                    break;
            }

            TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage, priority);
        }

        public void HelpItem(ChatMessage message)
        {
            // Cut out "!cheese help" start.
            String arguments = message.Message
                .StripStart("!cheese h")
                .StripStart("elp");

            Player player = GetPlayer(message);

            if (String.IsNullOrWhiteSpace(arguments))
            {
                TwitchClientManager.SpoolMessageAsMe(message.Channel, 
                    $"{player.Name} Commands: !cheese <command> where command is " +
                    $"| shop - look at what is available to buy with cheese " +
                    $"| buy <item> - buy an item at the shop " +
                    $"| help <item> - get information about an item in the shop " +
                    $"| quest - potentially get a big reward. The more workers you have, the greater chance of success. " +
                    $"| heist - Gamble some cheese, to potentially get more in return. " +
                    $"| rank - show information about your rank " +
                    $"| rankup - Spend cheese to unlock new items to buy at the shop.",
                    Priority.Low);
                return;
            }

            String itemToBuy = arguments[1..].ToLower();

            String outputMessage = itemToBuy switch
            {
                "s" or "storage" => $"Storage increases the maximum amount of cheese you can have.",
                "p" or "population" => $"Population increases the maximum number of workers you can have.",
                "w" or "worker" or "workers" => $"Workers increase the amount of cheese you get every time you gain cheese with \"!cheese\" and increase the success chance with you go on a quest with \"!cheese quest\".",
                "q" or "quest" or "quests" => $"Go on a random quest to get rewards or risk punishment. The chance of success scales with how many workers you have.",
                "recipe" or "recipes" => $"Recipes allow you to create new kinds of cheese with \"!cheese\".",
                "r" or "rank" or "ranks" => $"Ranks unlock new items to buy at the shop. Eventually ranking will give you prestige, reseting your rank and everything you have to restart the climb. For every prestige you gain, you get a permanent {(Int32)(Constants.PrestigeBonus * 100)}% boost to your cheese gains, which can stack.",
                "u" or "upgrade" or "upgrades" => $"Upgrades provide a permanent bonus to your cheese factory until you prestige.",
                "m" or "mouse" or "mousetrap" or "mousetraps" => $"Mousetraps kills giant rats that infest your cheese factory.",
                "c" or "cat" or "cats" => $"[CURRENTLY DO NOTHING] Cats help you fight against the giant evil mouse, Chubshan the Immortal. The more cats you have, the more you will be rewarded when Chubshan is defeated.",
                _ => $"Invalid item \"{itemToBuy}\" name. Type \"!cheese shop\" to see the items available for purchase.",
            };
            TwitchClientManager.SpoolMessageAsMe(message.Channel, player, outputMessage, Priority.Low);
        }
    }
}
