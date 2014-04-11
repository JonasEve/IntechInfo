using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    class StartingWithLinq
    {
        static IEnumerable<int> Numbers(int start, int count)
        {
            while (--count >= 0) yield return start++;
        }

        static IEnumerable<int> Sawtooth(int count)
        {
            int i = 0;
            while(true)
            {
                yield return i;
                i++;
                if (i > count)
                    i = 0;
            }
        }

        [Test]
        public void SimpleRanges()
        {
            CollectionAssert.AreEqual(Numbers(0, 5), new[] { 0, 1, 2, 3, 4 });

            Sawtooth(5).Skip(20).Limit(13).Count();
        }

        [Test]
        public void MiniLinqInAction()
        {
            var pairNumbers = Sawtooth(10).Where(n => n % 2 == 0);
        }
    }
}
