using System;
using Chubberino.Common.ValueObjects;

namespace Chubberino.Bots.Common.Commands.Settings.Replies
{
    public sealed class ContainsComparator : IContainsComparator
    {
        public LowercaseString Name { get; } = LowercaseString.From("contains");

        public Boolean Matches(String left, String right)
        {
            return left.Contains(right);
        }
    }
}
