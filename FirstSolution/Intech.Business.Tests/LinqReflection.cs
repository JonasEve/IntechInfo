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
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach(var element in @this)
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

            int paramCount = 1;

            methodInfo.Where(m => m.GetParameters().Length == paramCount).Select(m => m.Name).ForEach(Console.WriteLine);
        }

        [Test]
        public void SystemTextIAmUsing()
        {

        }
    }
}
