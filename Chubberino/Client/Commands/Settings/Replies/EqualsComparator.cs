using System;

namespace Chubberino.Client.Commands.Settings.Replies
{
    public sealed class EqualsComparator : IEqualsComparator
    {
        public String Name { get; } = "equals";

        public Boolean Matches(String left, String right)
        {
            return left == right;
        }
    }
}
