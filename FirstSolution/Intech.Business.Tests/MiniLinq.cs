using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    public static class MiniLinq
    {
        public static IEnumerable<TResult> Select<T, TResult>(this IEnumerable<T> enumeration, Func<T, TResult> map)
        {
            foreach (var obj in enumeration)
            {
                yield return (map(obj));
            }
        }
        public static IEnumerable<T> Where<T>(this IEnumerable<T> enumeration, Func<T,bool> filter)
        {
            foreach (var obj in enumeration)
            {
                if (filter(obj)) yield return obj;
            }
        }
        public static IEnumerable<T> Limit<T>(this IEnumerable<T> enumeration, int maxCount)
        {
            foreach (var obj in enumeration)
            {
                if (--maxCount >= 0) yield return obj;
                else break;
            }
        }

        public static IEnumerable<T> Skip<T>(this IEnumerable<T> enumeration, int count)
        {
            foreach (var obj in enumeration)
            {
                if (count >= 0)
                {
                    --count;
                    continue;
                }
                yield return obj;
            }
        }

        public static int Count(this IEnumerable e)
        {
            var enumerator = e.GetEnumerator();
            int count = 0;
            while (enumerator.MoveNext()) count++;
            return count;
        }
    }
}
