using Chubberino.Modules.CheeseGame.Items;
using TwitchLib.Client.Models;

namespace Chubberino.Modules.CheeseGame.Shops
{
    public interface IShop : ICommandStrategy
    {
        void ListInventory(ChatMessage message);

        void BuyItem(ChatMessage message);

        void HelpItem(ChatMessage message);

        IShop AddItem(IItem item);
    }
}
