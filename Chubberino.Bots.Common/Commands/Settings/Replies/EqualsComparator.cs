using Chubberino.Common.ValueObjects;

namespace Chubberino.Bots.Common.Commands.Settings.Replies;

public sealed class EqualsComparator : IEqualsComparator
{
    public Name Name { get; } = Name.From("equals");

    public Boolean Matches(String left, String right)
        => left == right;
}
