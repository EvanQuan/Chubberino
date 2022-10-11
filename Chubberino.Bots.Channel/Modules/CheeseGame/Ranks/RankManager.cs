using System.Collections.Generic;
using Chubberino.Bots.Channel.Modules.CheeseGame.Emotes;
using Chubberino.Bots.Channel.Modules.CheeseGame.Heists;
using Chubberino.Bots.Channel.Modules.CheeseGame.Items.Recipes;
using Chubberino.Common.Extensions;
using Chubberino.Database.Contexts;
using Chubberino.Database.Models;
using Chubberino.Infrastructure.Client;
using Chubberino.Infrastructure.Client.TwitchClients;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Ranks;

public class RankManager : IRankManager
{
    /// <summary>
    /// Bonus multiplier per every prestige level.
    /// </summary>
    public const Double PrestigeBonus = 0.1;

    public static IReadOnlyDictionary<Rank, Int32> RanksToPoints { get; } = new Dictionary<Rank, Int32>()
    {
        {  Rank.Bronze, 300 },
        {  Rank.Silver, 900 },
        {  Rank.Gold, 1800 },
        {  Rank.Platinum, 3600 },
        {  Rank.Diamond, 7200 },
        {  Rank.Master, 14400 },
        {  Rank.Grandmaster, 28800 },
        {  Rank.Legend, 57600 },
    };
    public IApplicationContextFactory ContextFactory { get; }
    public ITwitchClientManager Client { get; }
    public Random Random { get; }
    public IEmoteManager EmoteManager { get; }
    public IHeistManager HeistManager { get; }

    public RankManager(
        IApplicationContextFactory contextFactory,
        ITwitchClientManager client,
        Random random,
        IEmoteManager emoteManager,
        IHeistManager heistManager)
    {
        ContextFactory = contextFactory;
        Client = client;
        Random = random;
        EmoteManager = emoteManager;
        HeistManager = heistManager;
    }

    public void RankUp(ChatMessage message)
    {
        using var context = ContextFactory.GetContext();

        var player = context.GetPlayer(Client, message);

        String outputMessage;

        outputMessage = GetRankUpMessage(message, player, context, out var priority);

        Client.SpoolMessageAsMe(message.Channel, player, outputMessage, priority);
    }

    private String GetRankUpMessage(ChatMessage message, Player player, IApplicationContext context, out Priority priority)
    {
        priority = Priority.Low;

        if (RanksToPoints.TryGetValue(player.Rank, out Int32 pointsToRank))
        {
            Rank nextRank = player.Rank.Next();

            if (player.Points >= pointsToRank)
            {
                if (nextRank == Rank.None)
                {
                    if (player.HasUnlockedAllRecipes())
                    {
                        HeistManager.LeaveAllHeists(context, player);
                        // Prestige instead of rank up
                        player.ResetRank();
                        player.Prestige++;
                        context.SaveChanges();
                        var positiveEmotes = EmoteManager.Get(message.Channel, EmoteCategory.Positive);
                        priority = Priority.Medium;
                        return $"{Random.NextElement(positiveEmotes)} You prestiged back to {Rank.Bronze} and have gained a permanent {(Int32)(PrestigeBonus * 100)}% cheese gain boost. {Random.NextElement(positiveEmotes)}";
                    }

                    return $"You need to buy all cheese recipes in order to prestige back to {Rank.Bronze} rank. " +
                        $"You will lose all your cheese and upgrades, but will gain a permanent {(Int32)(PrestigeBonus * 100)}% bonus on your cheese gains.";
                }

                player.Points -= pointsToRank;
                player.Rank = nextRank;
                context.SaveChanges();
                priority = Priority.Medium;

                return $"You ranked up to {nextRank}. {Random.NextElement(EmoteManager.Get(message.Channel, EmoteCategory.Positive))} (-{pointsToRank} cheese)";

            }

            var pointsNeededToRank = pointsToRank - player.Points;
            if (nextRank == Rank.None)
            {
                return $"You need {pointsNeededToRank} more cheese in order to prestige back to {Rank.Bronze} rank. " +
                    $"You will lose all your cheese and upgrades, but will gain a permanent {(Int32)(PrestigeBonus * 100)}% bonus on your cheese gains.";
            }

            return $"You need {pointsNeededToRank} more cheese in order to rank up to {nextRank}.";
        }

        return $"Uh oh, you broke something. You have an invalid rank of {player.Rank}.";
    }

    public void ShowRank(ChatMessage message)
    {
        using var context = ContextFactory.GetContext();

        var player = context.GetPlayer(Client, message);

        String outputMessage = GetRankMessage(player);

        Client.SpoolMessageAsMe(message.Channel, player, outputMessage, Priority.Low);

    }

    private static String GetRankMessage(Player player)
    {
        var nextRank = player.Rank.Next();

        if (RanksToPoints.TryGetValue(player.Rank, out Int32 pointsToRank))
        {
            var nextRankInformation = new StringBuilder("You are currently in ")
                .Append(player.Rank)
                .Append(" rank. ");

            if (pointsToRank > player.Points)
            {
                nextRankInformation
                    .Append("You need ")
                    .Append(pointsToRank - player.Points)
                    .Append(" more cheese to rankup to ");
            }
            else
            {
                nextRankInformation
                    .Append("You have enough cheese (")
                    .Append(pointsToRank)
                    .Append(") to rankup right now to ");
            }

            if (nextRank == Rank.None)
            {
                nextRankInformation
                    .Append("prestige back to ")
                    .Append(Rank.Bronze)
                    .Append(" rank. You will lose all your cheese and upgrades, but will gain a permanent ")
                    .Append((Int32)(PrestigeBonus * 100))
                    .Append("% bonus on your cheese gains.");
            }
            else
            {
                nextRankInformation
                    .Append(nextRank)
                    .Append(" rank.");
            }

            return nextRankInformation.ToString();
        }

        return $"Uh oh, you broke something. You have an invalid rank of {player.Rank}.";
    }
}
