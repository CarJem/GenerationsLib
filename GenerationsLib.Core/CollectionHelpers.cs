using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationsLib.Core
{
    public static class CollectionHelpers
    {
        public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (elementSelector == null)
            {
                throw new ArgumentNullException("elementSelector");
            }

            var dictionary = new Dictionary<TKey, TElement>();
            foreach (TSource current in source)
            {
                dictionary.Add(keySelector(current), elementSelector(current));
            }
            return dictionary;
        }
    }
}
