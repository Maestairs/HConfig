using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HConfig;
using NUnit.Framework;

namespace HConfigTests
{
    [TestFixture]
    public class ControlledConfigPlaneTests
    {
        [Test]
        public void NewControlledConfigPlaneHasNullChild()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");
            Assert.IsNull(sut.Child);
        }


        [Test]
        public void NewControlledConfigPlaneHasAConfigPlane()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");
            Assert.IsNotNull(sut.Plane);
        }
  
    }
}
