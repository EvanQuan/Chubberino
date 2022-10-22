using System.IO;
using Chubberino.Bots.Channel.Modules.CheeseGame.Heists;
using Chubberino.Bots.Channel.Modules.CheeseGame.Helping;
using Chubberino.Bots.Channel.Modules.CheeseGame.Points;
using Chubberino.Bots.Channel.Modules.CheeseGame.Quests;
using Chubberino.Bots.Channel.Modules.CheeseGame.Ranks;
using Chubberino.Bots.Channel.Modules.CheeseGame.Shops;
using Chubberino.Infrastructure.Client.TwitchClients;

namespace Chubberino.Bots.Channel.Commands
{
    internal class Cheese2 : Cheese
    {
        public Cheese2(
            ITwitchClientManager client,
            TextWriter writer,
            IHelpManager help,
            IPointManager pointManager,
            IShop shop,
            IRankManager rankManager,
            IQuestManager questManager,
            IHeistManager heistManager)
            : base(
                client,
                writer,
                help,
                pointManager,
                shop,
                rankManager,
                questManager,
                heistManager)
        {
        }
    }
}
