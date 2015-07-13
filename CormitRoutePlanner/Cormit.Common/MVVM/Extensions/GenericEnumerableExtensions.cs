#region

using System;
using System.Collections.Generic;

#endregion

namespace Imarda.Lib.MVVM.Extensions
{
    public static class GenericEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> me, Action<T> action)
        {
            foreach (var item in me)
            {
                action(item);
            }
        }
    }
}