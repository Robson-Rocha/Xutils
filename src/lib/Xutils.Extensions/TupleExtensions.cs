namespace Xutils.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class TupleExtensions
    {
        public static void AddNew<T1, T2>(this ICollection<Tuple<T1, T2>> tuples, T1 item1, T2 item2)
        {
            tuples.Add(new Tuple<T1, T2>(item1, item2));
        }
    }
}
