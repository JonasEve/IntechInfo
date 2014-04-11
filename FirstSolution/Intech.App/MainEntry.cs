using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intech.Business;
using System.Reflection;

namespace Intech.App
{

    public static class MyLinqExtension
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (var element in @this)
            {
                action(element);
            }
        }
    }
    class MainEntry
    {   
        static void Main( string[] args )
        {
            Assembly a = Assembly.GetExecutingAssembly();
            var methodInfo = a.GetTypes().SelectMany(t => t.GetMethods());

            int paramCount = 1;

            methodInfo.Where(m => m.GetParameters().Length == paramCount).Select(m => m.Name).ForEach(Console.WriteLine);

            System.Console.ReadKey();

            //FileProcessor p = new FileProcessor();
            //var r = p.Process("C:\\AdwCleaner");
            //Console.WriteLine( "TotalFileCount = {0}", r.TotalFileCount );
            //Console.WriteLine( "TotalDirectoryCount = {0}", r.TotalDirectoryCount );
            //Console.WriteLine( "HiddenFileCount = {0}", r.HiddenFileCount );
            //Console.WriteLine( "HiddenDirectoryCount = {0}", r.HiddenDirectoryCount );
            //Console.WriteLine( "There is {0} unaccessible file(s).", r.UnaccessibleFileCount );
            //Console.ReadKey();
        }
    }
}
