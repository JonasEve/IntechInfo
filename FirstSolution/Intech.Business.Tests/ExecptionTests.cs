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

            Console.WriteLine("with : {0}, without : {1}, ratio {2}", withException, withoutException, withException / withoutException);
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


        string BuildNaive(string pattern, int count)
        {
            string s = String.Empty;
            while (--count > 0)
            {
                s += pattern;
            }

            return s;
        }

        string BuildBetter(string pattern, int count)
        {
            StringBuilder sb = new StringBuilder();
            while (--count > 0)
            {
                sb.Append(pattern);
            }

            return sb.ToString();
        }

        [Test]
        public void Complexity()
        {
            int max = 10000;

            for (int i = 0; i < max; i += 100)
            {
                BuildString(i);
            }
        }

        void BuildString(int count)
        {
            int testcount = 10;
            string pattern = "toto";

            Stopwatch w = new Stopwatch();

            long naiveTicks;

            w.Start();
            for (int i = 0; i < testcount; i++)
            {
                BuildNaive(pattern, count);
            }
            w.Stop();
            naiveTicks = w.ElapsedTicks;

            long betterTicks;
            w.Restart();
            for (int i = 0; i < testcount; i++)
            {
                BuildBetter(pattern, count);
            }
            w.Stop();
            betterTicks = w.ElapsedTicks;

            Console.WriteLine("naive : {0}, better : {1}, ratio {2}", naiveTicks, betterTicks, naiveTicks / betterTicks);
        }
    }
}
