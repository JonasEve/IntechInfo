using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    public static class MyLinqExtension
    {
        public static void ForEach<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> @this, Action<TKey> open, Action<TValue> read, Action close)
        {
            foreach(var element in @this)
            {
                open(element.Key);
                element.ForEach(read);
                close();
            }
        }

        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var element in @this)
            {
                action(element);
            }
        }
    }

    [TestFixture]
    public class LinqReflection
    {
        [Test]
        public void AllTheMethodIAmImplement()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            var methodInfo = a.GetTypes().SelectMany(t => t.GetMethods());

            methodInfo.GroupBy(m => m.GetParameters().Length).OrderBy(m => m.Key)
                .ForEach(Console.WriteLine
                , Console.WriteLine
                , Console.WriteLine);

        }
        [Test]
        public void SystemTextIAmUsing()
        {

        }
    }
}
