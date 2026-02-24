using UnityEngine;
using System.Collections.Generic;

public static class ListExtensions
{
    public static T Draw<T>(this IList<T> list)
    {
        if (list.Count == 0)
        {
            return default;
        }
        int r = Random.Range(0, list.Count);
        T item = list[r];
        list.Remove(item);
        return item;
    }
}