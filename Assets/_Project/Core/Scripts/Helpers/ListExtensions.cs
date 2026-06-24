using System.Collections.Generic;
using UnityEngine;

namespace Match3.Core.Helpers
{
    public static class ListExtensions
    {
        public static T SelectRandomAndRemove<T>(this List<T> list)
        {
            var randomIndex = Random.Range(0, list.Count);
            var selectedItem = list[randomIndex];
            list[randomIndex] = list[^1];
            list.RemoveAt(list.Count - 1);
            return selectedItem;
        }
    }
}