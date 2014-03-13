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
    public class FirstTests
    {
        [Test]
        public void Test1()
        {
            //Arrange
            int firstNumber = 1;
            int secondNumber = 2;
            int sum;

            //Act
            sum = firstNumber + secondNumber;

            //Assert
            Assert.That(sum > firstNumber && sum > secondNumber);
        }

        [Test]
        public void WhereAmI()
        {
            DirectoryInfo directory = TestHelper.SolutionFolder;
        }
    }
}
