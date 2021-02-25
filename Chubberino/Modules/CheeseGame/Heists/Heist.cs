using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using Chubberino.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Heists
{
    public sealed class Heist : IHeist
    {
        public const Double MinimumWinnerPercent = 0.33;
        public const Double MaximumWinnerPercent = 1;

        private IList<(Int32 PlayerID, String PlayerName, Int32 WageredPoints)> Wagers { get; }

        public Heist(
            ChatMessage message,
            IApplicationContext context,
            Random random,
            ITwitchClientManager client)
        {
            Wagers = new List<(Int32, String, Int32)>();
            InitiatorMessage = message;
            InitiatorName = message.DisplayName;
            Context = context;
            Random = random;
            TwitchClient = client;
        }

        public ChatMessage InitiatorMessage { get; }
        public IApplicationContext Context { get; }
        public Random Random { get; }
        public ITwitchClientManager TwitchClient { get; }

        public String InitiatorName { get; }

        public Boolean Start()
        {
            if (Wagers.Count == 0)
            {
                TwitchClient.SpoolMessageAsMe(InitiatorMessage.Channel, "No one joined the heist.");
                return false;
            }

            String others = Wagers.Count switch
            {
                1 => "goes",
                2 => $"and {Wagers[1].PlayerName} go",
                _ => $"and {Wagers.Count} others go"
            };

            var intro = new StringBuilder($"{Wagers[0].PlayerName} {others} into the lair of the great cheese dragon. ");

            Double winnerPercent = Random.NextDouble(MinimumWinnerPercent, MaximumWinnerPercent);

            // Convert.ToInt32 will round up to the nearest Int32 instead of truncating with casting,
            // so a single wager will still have a chance to fail or succeed randomly.
            Int32 winnerCount = Convert.ToInt32(winnerPercent * Wagers.Count);


            if (winnerCount == 0)
            {
                intro.Append("Unfortunately the cheese dragon prevented anyone from getting anything");
                TwitchClient.SpoolMessage(InitiatorMessage.Channel, intro.ToString());
                return true;
            }


            if (winnerCount == Wagers.Count)
            {
                intro.Append("Everyone made it out with the spoils! ");
            }
            else
            {
                intro.Append("Some made it out with the spoils! ");
            }

            var winners = new List<(Int32, String, Int32)>();

            winnerCount.Repeat(() => winners.Add(Random.RemoveElement(Wagers)));

            foreach ((Int32 playerID, String playerName, Int32 wageredPoints) in winners)
            {
                var player = Context.GetPlayer(TwitchClient, InitiatorMessage);
                Int32 winnerPoints = (Int32)((1.0 / winnerPercent + 0.5) * wageredPoints);
                player.AddPoints(winnerPoints);
                Context.SaveChanges();
                intro.Append($"{player.Name} (+{winnerPoints}) ");
            }

            TwitchClient.SpoolMessage(InitiatorMessage.Channel, intro.ToString());

            return true;
        }

        public void UpdateWager(Player player, Int32 points)
        {
            String updateMessage;
            Int32 updatedPoints = Math.Min(player.Points, points);

            if (Wagers.TryGetFirst(x => x.PlayerID == player.ID, out var wager))
            {
                if (updatedPoints <= 0)
                {
                    Wagers.Remove(wager);
                    updateMessage = $"You left the heist.";
                }

                wager.WageredPoints = updatedPoints;
                updateMessage = $"You updated your heist wager to {wager.WageredPoints} cheese.";
            }
            else if (updatedPoints <= 0)
            {
                updateMessage = $"You must wager a positive number of cheese to join the heist.";
            }
            else
            {
                Wagers.Add((player.ID, player.Name, updatedPoints));
                updateMessage = $"You joined the heist, wagering {wager.WageredPoints} cheese.";
            }

            TwitchClient.SpoolMessageAsMe(InitiatorMessage.Channel, player, updateMessage);
        }
    }
}
