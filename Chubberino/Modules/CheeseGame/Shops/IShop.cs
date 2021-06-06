using Chubberino.Modules.CheeseGame.Items;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Shops
{
    public interface IShop
    {
        void ListInventory(ChatMessage message);

        void BuyItem(ChatMessage message);

        IShop AddItem(IItem item);
    }
}
