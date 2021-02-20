using System;

namespace Chubberino.Modules.CheeseGame.Emotes
{
    public interface IEmoteManager
    {
        String GetRandomPositiveEmote(Boolean useChannelEmotes);

        String GetRandomNegativeEmote(Boolean useChannelEmotes);
    }
}
