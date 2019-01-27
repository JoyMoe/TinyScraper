using System.Collections.Generic;

namespace TinyScraper
{
    public static class Utilities
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> source)
        {
            if (source == null) return;

            foreach (var item in source)
            {
                collection.Add(item);
            }
        }
    }
}
