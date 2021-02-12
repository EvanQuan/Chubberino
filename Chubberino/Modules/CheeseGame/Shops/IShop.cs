using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Shops
{
    public interface IShop
    {
        public void ListInventory(ChatMessage message);

        public void BuyItem(ChatMessage message);

        public void HelpItem(ChatMessage message);
    }
}
