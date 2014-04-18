using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class EncodingTests
    {
        public void EncodingBasics()
        {
            string s = "Toto";
            byte[] b0 = Encoding.Unicode.GetBytes(s);
            byte[] b1 = Encoding.UTF32.GetBytes(s);
            byte[] b2 = Encoding.UTF8.GetBytes(s);
            byte[] b3 = Encoding.UTF7.GetBytes(s);
            byte[] b4 = Encoding.BigEndianUnicode.GetBytes(s);
        }
    }
}
