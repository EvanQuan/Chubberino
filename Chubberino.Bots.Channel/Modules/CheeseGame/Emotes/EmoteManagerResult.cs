using System.Collections.Generic;

namespace Chubberino.Bots.Channel.Modules.CheeseGame.Emotes;

public readonly struct EmoteManagerResult
{
    public EmoteManagerResult(IEnumerable<String> succeeded, IEnumerable<String> failed)
    {
        Succeeded = succeeded;
        Failed = failed;
    }

    public IEnumerable<String> Succeeded { get; }
    public IEnumerable<String> Failed { get; }
}
