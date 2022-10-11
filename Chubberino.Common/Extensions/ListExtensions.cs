namespace Chubberino.Common.Extensions;

public static class ListExtensions
{
    public static Option<TType> TryGet<TType>(this IList<TType> list, Int32 index)
    {
        if (0 > index || index >= list.Count)
        {
            return Option<TType>.None;
        }

        return list[index];
    }

    public static Option<TType> TryGet<TType>(this IReadOnlyList<TType> list, Int32 index)
    {
        if (0 > index || index >= list.Count)
        {
            return Option<TType>.None;
        }

        return list[index];
    }
}
