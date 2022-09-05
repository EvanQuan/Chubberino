using System;
using System.Collections.Generic;

namespace Chubberino.Common.Extensions;

public static class IListExtensions
{
    public static Boolean TryGet<TType>(this IList<TType> list, Int32 index, out TType element)
    {
        if (0 > index || index >= list.Count)
        {
            element = default;
            return false;
        }

        element = list[index];
        return true;
    }

    public static Boolean TryGet<TType>(this IReadOnlyList<TType> list, Int32 index, out TType element)
    {
        if (0 > index || index >= list.Count)
        {
            element = default;
            return false;
        }

        element = list[index];
        return true;
    }
}
