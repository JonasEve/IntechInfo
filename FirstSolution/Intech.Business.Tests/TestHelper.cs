using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    class TestHelper
    {
        static public DirectoryInfo TestSupportFolder
        {
            get
            {
                return new DirectoryInfo(
                                Path.Combine(
                                    SolutionFolder.FullName,
                                    "Intech.Business.Tests",
                                    "TestSupport"));
            }
        }

        static public DirectoryInfo SolutionFolder
        {
            get
            {
                return new DirectoryInfo(Path.GetDirectoryName(
                                            Path.GetDirectoryName(
                                                Path.GetDirectoryName(
                                                    Path.GetDirectoryName(
                                                        new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath)))));
            }
        }
    }
}
