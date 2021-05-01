using System;
using System.Collections.Generic;

namespace Chubberino.Modules.CheeseGame.Emotes
{
    public struct EmoteManagerResult
    {
        public EmoteManagerResult(IEnumerable<String> succeeded, IEnumerable<String> failed)
        {
            Succeeded = succeeded;
            Failed = failed;
        }

        public IEnumerable<String> Succeeded { get; }
        public IEnumerable<String> Failed { get; }
    }
}
