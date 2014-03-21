using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class ExecptionTests
    {
        #region exception
        bool _boolBHasBeenCall;

        [Test]
        public void simpleScenario()
        {
            _boolBHasBeenCall = false;

            try
            {
                FunctionA();
                FunctionB();
            }
            catch
            {
            }
            finally
            {
                Assert.That(_boolBHasBeenCall, Is.False);
            }

        }

        void FunctionA()
        {
            throw new Exception();
        }

        void FunctionB()
        {
           _boolBHasBeenCall = true;
        }
        #endregion

        [Test]
        public void smallPerfTest()
        {
            ParseWithoutException("vfsv");
            ParseWithoutException("123");
        }

        static void ParseWithoutException(String item)
        {
            String toParse = item;

            Stopwatch w = new Stopwatch();

            long withException = ParseWithException(w, toParse);

            w.Restart();
            for (int i = 0; i < 100; i++)
            {
                int result;
                Int32.TryParse(toParse, out result);
            }
            w.Stop();

            long withoutException = w.ElapsedTicks;

            Console.WriteLine("with : {0}, without : {1}, ratio {2}", withException, withoutException, withException / withoutException); ;
        }

        static long ParseWithException(Stopwatch w, String toParse)
        {
            w.Start();
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    Int32.Parse(toParse);
                }
                catch { }
            }
            w.Stop();
            long ticks = w.ElapsedTicks;
            return ticks;
        }
    }
}
