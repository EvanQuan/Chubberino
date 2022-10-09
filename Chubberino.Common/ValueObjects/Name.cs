namespace Chubberino.Common.ValueObjects;

public readonly record struct Name
{
    public static Name From(String value)
        => new(value);

    private Name(String value)
        => Value = value.Any(Char.IsUpper)
            ? value.ToLower()
            : value;

    public String Value { get; }

    public override String ToString()
    {
        return Value;
    }
}
