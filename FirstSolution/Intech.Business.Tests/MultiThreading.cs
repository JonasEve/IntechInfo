using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class MultiThreading
    {
        [Test]
        public void ThreadCreation()
        {
            var t = new Thread (ThreadFunc);
            t.Name = "My first thread";
            t.Start();
            Console.WriteLine("Finished");
            ManyLines();
        }

        private void ManyLines()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine("{0} says {1}", Thread.CurrentThread.Name, i);
                Thread.Sleep(0);
            }
        }

        void ThreadFunc()
        {
            ManyLines();
        }
    }
}
