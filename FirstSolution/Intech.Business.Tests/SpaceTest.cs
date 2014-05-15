using Intech.Space;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business.Tests
{
    [TestFixture]
    public class SpaceTest
    {
        [Test]
        public void CreateOneUniverse()
        {
            var u = new Universe();
            u.AddGalaxy("test");
            u.AddGalaxy("test2");
            u["test"].RenameGalaxy("truc");
            u["truc"].AddStar("star");
            u["truc"].Stars.Where(s => s.Name == "star").SingleOrDefault().Name = "star2";

            Assert.That(u["truc"].Name, Is.EqualTo("truc"));

            var u2 = new Universe();
            u["truc"].MoveTo(u2);

            Assert.That(u2["truc"].Name, Is.EqualTo("truc"));

            u2["truc"].Implode();

            Assert.That(u2.GetNumberOfGalaxies(), Is.EqualTo(0));
        }
    }
}
