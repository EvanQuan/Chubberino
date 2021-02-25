using Chubberino.Client;
using Chubberino.Database.Contexts;
using Chubberino.Modules.CheeseGame.Models;
using Chubberino.Modules.CheeseGame.PlayerExtensions;
using System.Linq;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame
{
    public static class IApplicationContextExtensions
    {
        public static Player GetPlayer(this IApplicationContext source, ITwitchClientManager client, ChatMessage message)
        {
            var player = source.Players.FirstOrDefault(x => x.TwitchUserID == message.UserId);

            if (player == null)
            {
                player = new Player()
                {
                    TwitchUserID = message.UserId,
                    Name = message.DisplayName
                }
                .ResetRank();

                source.Add(player);

                source.SaveChanges();

                client.SpoolMessageAsMe(message.Channel, player, $"!!! NEW CHEESE FACTORY !!!You have just begun building your own cheese factory in the lands of Kashkaval, where {player.ID - 1} other cheese factories already reside here. Begin producing cheese with \"!cheese\". You can get help with \"!cheese help\". Good luck!");
            }

            return player;
        }
    }
}
