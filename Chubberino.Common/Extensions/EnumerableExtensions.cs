namespace Chubberino.Common.Extensions;

public static class EnumerableExtensions
{
    public static Option<TElement> TryGetFirst<TElement>(this IEnumerable<TElement> source, Func<TElement, Boolean> predicate)
        => source.Any(predicate)
            ? (Option<TElement>)source.FirstOrDefault(predicate)
            : Option<TElement>.None;

    public static Option<TElement> TryGetFirst<TElement>(this IEnumerable<TElement> source)
        => source.Any()
            ? (Option<TElement>)source.FirstOrDefault()
            : Option<TElement>.None;

    public static Option<(TElement Element, IEnumerable<TElement> Next)> TryGetFirstAndNext<TElement>(this IEnumerable<TElement> source)
        => source.TryGetFirst()
            .Some(first =>
            {
                var next = source.Skip(1);
                return Option<(TElement, IEnumerable<TElement>)>.Some((first, next));
            })
            .None(Option<(TElement, IEnumerable<TElement>)>.None);

    public static IEnumerable<TType> ForEach<TType>(this IEnumerable<TType> source, Action<TType> action)
    {
        foreach (var element in source)
        {
            action(element);
        }

        return source;
    }

    public static String ToLineDelimitedString<TType>(this IEnumerable<TType> source, Int32 tabCount)
    {
        String delimiter = Environment.NewLine + new String('\t', tabCount);
        return String.Join(delimiter, source);
    }
}
