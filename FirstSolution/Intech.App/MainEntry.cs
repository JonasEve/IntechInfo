using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intech.Business;

namespace Intech.App
{
    class MainEntry
    {
        static void Main( string[] args )
        {
            FileProcessor p = new FileProcessor();
            var r = p.Process("C:\\AdwCleaner");
            Console.WriteLine( "TotalFileCount = {0}", r.TotalFileCount );
            Console.WriteLine( "TotalDirectoryCount = {0}", r.TotalDirectoryCount );
            Console.WriteLine( "HiddenFileCount = {0}", r.HiddenFileCount );
            Console.WriteLine( "HiddenDirectoryCount = {0}", r.HiddenDirectoryCount );
            Console.WriteLine( "There is {0} unaccessible file(s).", r.UnaccessibleFileCount );
            Console.ReadKey();
        }
    }
}
