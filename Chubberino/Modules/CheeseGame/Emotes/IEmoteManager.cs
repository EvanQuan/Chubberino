using System;

namespace Chubberino.Modules.CheeseGame.Emotes
{
    public interface IEmoteManager
    {
        String GetRandomPositiveEmote();

        String GetRandomNegativeEmote();
    }
}
