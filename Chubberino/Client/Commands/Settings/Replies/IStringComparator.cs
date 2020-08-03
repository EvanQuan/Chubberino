using System;

namespace Chubberino.Client.Commands.Settings.Replies
{
    public interface IStringComparator
    {
        String Name { get; }

        Boolean Matches(String left, String right);
    }
}
