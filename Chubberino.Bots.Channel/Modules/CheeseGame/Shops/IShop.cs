using Chubberino.Bots.Channel.Modules.CheeseGame.Items;
using TwitchLib.Client.Models;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Shops;

public interface IShop
{
    IItem[] Items { get; set; }

    void ListInventory(ChatMessage message);

    void BuyItem(ChatMessage message);
}
