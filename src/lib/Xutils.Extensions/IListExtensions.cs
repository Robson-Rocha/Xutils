namespace Xutils.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class IListExtensions
    {
        public static void Resize<T>(this IList<T> list, int newSize)
            where T : new()
        {
            Resize(list, newSize, (c) => new T());
        }

        public static void Resize<T>(this IList<T> list, int newSize, Func<IList<T>, T> itemFactory)
        {
            if (list.Count < newSize)
            {
                for (int i = 0, l = newSize - list.Count; i < l; i++)
                    list.Add(itemFactory(list));
            }
            if (list.Count > newSize)
            {
                for (int i = 0, l = list.Count - newSize; i < l; i++)
                    list.RemoveAt(list.Count - 1);
            }
        }
    }
}
