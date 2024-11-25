﻿namespace RLE.Core.Extensions;

public static class EnumerableExtensions
{
    public static void AddRange<T>(this IList<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
}
