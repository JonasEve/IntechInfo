using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class MyDictionaryTests
    {
        int numberOfKey = 1000;
        MyDictionary<int, string> dico;
        [SetUp]
        public void InitMyDitionary()
        {
            dico = new MyDictionary<int, string>();
            for(int i = 0; i < numberOfKey; i++)
            {
                dico.Add(i, i + ",test" + i);
            }

            Assert.That(dico.Count, Is.EqualTo(1000));
        }
        [Test]
        public void AddTest()
        {
            dico.Add(1000, "1000,test1000");
            Assert.That(dico.Count, Is.EqualTo(1001));
        }

        [Test]
        public void ValueTest()
        {
            for(int i = 0; i < numberOfKey; i++)
            {
                Assert.That(dico[i], Is.EqualTo(i + ",test" + i));
            }
        }

        [Test]
        public void RemoveTest()
        {
            dico.Remove(999);

            Assert.That(dico.Count, Is.EqualTo(999));

            for (int i = 0; i < numberOfKey - 1; i++)
            {
                Assert.That(dico[i], Is.EqualTo(i + ",test" + i));
            }

            dico.Remove(1);

            Assert.That(dico.Count, Is.EqualTo(998));

            for (int i = 2; i < numberOfKey - 1; i++)
            {
                Assert.That(dico[i], Is.EqualTo(i + ",test" + i));
            }

            try
            {
                string value = dico[1];
            }
            catch(KeyNotFoundException e)
            {
                Assert.IsTrue(e != null);
            }
        }

        [Test]
        public void IteratorTest()
        {
            int i = 0;

            foreach(var pair in dico)
            {
                Assert.That(dico[pair.Key], Is.EqualTo(pair.Value));
                i++;
            }

            Assert.That(i, Is.EqualTo(1000));
        }


    }
}
