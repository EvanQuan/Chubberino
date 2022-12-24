namespace Chubberino.Common.ValueObjects;

public readonly record struct Name
{
    private Name(String value)
        => Value = value.Any(Char.IsUpper)
            ? value.ToLower()
            : value;

    public String Value { get; }

    public override String ToString()
        => Value;

    public static implicit operator String(Name value)
    {
        return value.Value;
    }

    public static implicit operator Name(String value)
    {
        return new(value);
    }
}
