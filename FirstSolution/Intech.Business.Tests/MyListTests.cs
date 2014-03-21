using Intech.Business;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class MyListTests
    {
        [Test]
        public void test1()
        {
            MyList<int> listInt = new MyList<int>();
            MyList<string> listString = new MyList<string>();
            MyList<MyList<string>> listListString = new MyList<MyList<string>>();
        }

        [Test]
        public void AssItems()
        {
            MyList<int> listInt = new MyList<int>();
            Assert.That(listInt.Count, Is.EqualTo(0));

            listInt.Add(1);

            Assert.That(listInt.Count, Is.EqualTo(1));
            Assert.That(listInt[0], Is.EqualTo(1));

            listInt.RemoveAt(0);
            Assert.That(listInt.Count, Is.EqualTo(0));

        }

        [Test]
        public void TraverseingItem()
        {
            var l = new MyList<int>();
            l.Add(5);
            l.Add(10);
            int sum = 0;

            foreach (var i in l)
            {
                sum += i;
            }

            Assert.That(sum, Is.EqualTo(15));
        }

        [Test]
        public void EmptyList()
        {
            var l = new MyList<int>();

            foreach (var i in l)
            {
                Assert.That(false);
            }

            Assert.That(true);
        }

        [Test]
        public void BigTest()
        {

        }
    }
}
