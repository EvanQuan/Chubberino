namespace Chubberino.Common.Extensions;

public static class ArrayExtensions
{
    public static Option<TType> TryGet<TType>(this TType[] array, Int32 index)
    {
        if (0 > index || index >= array.Length)
        {
            return Option<TType>.None;
        }

        return array[index];
    }
}
