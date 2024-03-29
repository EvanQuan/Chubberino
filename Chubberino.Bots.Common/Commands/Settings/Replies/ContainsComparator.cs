﻿using Chubberino.Common.ValueObjects;

namespace Chubberino.Bots.Common.Commands.Settings.Replies;

public sealed class ContainsComparator : IContainsComparator
{
    public Name Name { get; } = "contains";

    public Boolean Matches(String left, String right)
        => left.Contains(right);
}
