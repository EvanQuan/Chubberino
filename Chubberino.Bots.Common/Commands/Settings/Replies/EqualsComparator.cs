using System;
using Chubberino.Common.ValueObjects;

namespace Chubberino.Bots.Common.Commands.Settings.Replies
{
    public sealed class EqualsComparator : IEqualsComparator
    {
        public LowercaseString Name { get; } = LowercaseString.From("equals");

        public Boolean Matches(String left, String right)
        {
            return left == right;
        }
    }
}
