using System;

namespace Chubberino.Client.Commands.Settings.Replies
{
    public sealed class ContainsComparator : IContainsComparator
    {
        public String Name { get; } = "contains";

        public Boolean Matches(String left, String right)
        {
            return left.Contains(right);
        }
    }
}
