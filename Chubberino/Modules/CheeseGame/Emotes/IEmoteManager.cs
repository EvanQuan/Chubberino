using System;

namespace Chubberino.Modules.CheeseGame.Emotes
{
    public interface IEmoteManager
    {
        String GetRandomPositiveEmote(String channelName);

        String GetRandomNegativeEmote(String channelName);
    }
}
