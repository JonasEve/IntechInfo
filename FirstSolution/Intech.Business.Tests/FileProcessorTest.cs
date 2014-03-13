using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class FileProcessorTest
    {
        [Test]
        public void DirectoryNotExist()
        {
            FileProcessor process = new FileProcessor();

            FileProcessorResult result = process.Process("Nimp");

            Assert.That(!result.RootPathExists);
            Assert.That(result.TotalFileCount, Is.EqualTo(0));
        }

        [Test]
        public void DirectoryExist()
        {
            FileProcessor process = new FileProcessor();

            FileProcessorResult result = process.Process(Path.Combine(TestHelper.TestSupportPath, "EmptyFolder"));

            Assert.That(result.RootPathExists);
            Assert.That(result.TotalFileCount, Is.EqualTo(0));
            Assert.That(result.TotalDirectoryCount, Is.EqualTo(1));
        }
    }
}
