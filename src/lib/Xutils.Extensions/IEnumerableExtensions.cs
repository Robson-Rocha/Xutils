namespace Xutils.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Slices an IEnumerable in an list of IEnumerables of given size
        /// </summary>
        /// <typeparam name="T">Type of IEnumerable to be sliced</typeparam>
        /// <param name="data">IEnumerable to be sliced</param>
        /// <param name="sliceSize">Size of the slices</param>
        /// <returns>IEnumerable of IEnumerables of T with given slice size</returns>
        public static IEnumerable<IEnumerable<T>> Slice<T>(this IEnumerable<T> data, int sliceSize)
        {
            var enumerator = data.GetEnumerator();

            IEnumerable<T> NextSlice()
            {
                do
                    yield return enumerator.Current;
                while (--sliceSize > 0 && enumerator.MoveNext());
            }

            while (enumerator.MoveNext())
                yield return NextSlice();
        }

        /// <summary>
        /// Check if the <see cref="IEnumerable{T}"/> contains any duplicated elements, using the <typeparamref name="T"/> default <see cref="EqualityComparer{T}"/>
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="items">The IEnumerable to be checked for duplicates</param>
        /// <returns><code>true</code> if any duplicated member is found, <code>false</code> otherwise</returns>
        public static bool HasDuplicates<T>(this IEnumerable<T> items) => HasDuplicates(items, EqualityComparer<T>.Default);

        /// <summary>
        /// Check if the <see cref="IEnumerable{T}"/> contains any duplicated elements, using the provided <see cref="EqualityComparer{T}"/>
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="items">The <see cref="IEnumerable{T}"/> to be checked for duplicates</param>
        /// <param name="equalityComparer">The <see cref="EqualityComparer{T}"/> to be used to compare the items in the provided <see cref="IEnumerable{T}"/></param>
        /// <returns><code>true</code> if any duplicated member is found, <code>false</code> otherwise</returns>
        public static bool HasDuplicates<T>(this IEnumerable<T> items, IEqualityComparer<T> equalityComparer) => Duplicates(items, equalityComparer).Any();

        /// <summary>
        /// Get the duplicated items from the <see cref="IEnumerable{T}"/>, using the <typeparamref name="T"/> default <see cref="EqualityComparer{T}"/>
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="items">The IEnumerable to be checked for duplicates</param>
        /// <returns><see cref="IEnumerable{T}"/> containing the duplicated items</returns>
        public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> items) => Duplicates(items, EqualityComparer<T>.Default);

        /// <summary>
        /// Get the duplicated items from the <see cref="IEnumerable{T}"/>, using the provided <see cref="EqualityComparer{T}"/>
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="items">The IEnumerable to be checked for duplicates</param>
        /// <returns><see cref="IEnumerable{T}"/> containing the duplicated items</returns>
        public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
        {
            var set = new HashSet<T>();
            foreach (var item in items)
                if (!set.Add(item))
                    yield return item;
        }
    }
}
