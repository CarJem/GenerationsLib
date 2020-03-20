using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerationsLib.Core
{
    public static class ListHelpers
    {
        /// <summary>
        ///   Checks whether all items in the enumerable are same (Uses <see cref="object.Equals(object)" /> to check for equality)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>
        ///   Returns true if there is 0 or 1 item in the enumerable or if all items in the enumerable are same (equal to
        ///   each other) otherwise false.
        /// </returns>
        public static bool AreAllSame<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

            using (var enumerator = enumerable.GetEnumerator())
            {
                var toCompare = default(T);
                if (enumerator.MoveNext())
                {
                    toCompare = enumerator.Current;
                }

                while (enumerator.MoveNext())
                {
                    if (toCompare != null && !toCompare.Equals(enumerator.Current))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public static int MoveSelectedIndex(int listCount, int oldIndex, int newIndex)
        {
            if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= listCount) || (0 > newIndex) || (newIndex >= listCount)) return oldIndex;
            else return newIndex;
        }
        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
        {
            T aux = list[newIndex];
            list[newIndex] = list[oldIndex];
            list[oldIndex] = aux;
        }
        public static TValue GetSafe<TItem, TValue>(this IList<TItem> list, int index, Func<TItem, TValue> selector, TValue defaultValue)
        {
            // other checks omitted
            if (index < 0 || index >= list.Count)
            {
                return defaultValue;
            }
            return selector(list[index]);
        }
        #region TryGetElement
        public static T TryGetElement<T>(this T[] array, int index, T defaultElement)
        {
            if (index >= 0 && index < array.Length)
            {
                return array[index];
            }
            return defaultElement;
        }
        public static T TryGetElementNullable<T>(this T[] array, int index)
        {
            if (index >= 0 && index < array.Length)
            {
                return array[index];
            }
            return default(T);
        }
        public static TItem TryGetElement<TItem>(this IList<TItem> list, int index, TItem defaultElement)
        {
            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }
            return defaultElement;
        }

        public static TItem TryGetElementNullable<TItem>(this IList<TItem> list, int index)
        {
            if (index >= 0 && index < list.Count)
            {
                return list[index];
            }
            return default(TItem);
        }
        #endregion

        #region TrySetElement
        public static void TrySetElement<T>(this T[] array, int index, T value)
        {
            if (index >= 0 && index < array.Length)
            {
                array[index] = value;
            }
        }
        public static void TrySetElement<TItem>(this IList<TItem> list, int index, TItem value)
        {
            if (index >= 0 && index < list.Count)
            {
                list[index] = value;
            }
        }
        #endregion

        #region OutOfRangeCheck

        public static bool IsInRange<T>(this T[] array, int index)
        {
            if (index >= 0 && index < array.Length)
            {
                return true;
            }
            return false;
        }
        public static bool IsInRange<TItem>(this IList<TItem> list, int index)
        {
            if (index >= 0 && index < list.Count)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
