using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HConfig;
using NUnit.Framework;
using Rhino.Mocks;

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
        public void GetConfig_ReturnsValueIfFoundInPlane()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.Context = "MyContext";
            string configValue = sut.GetConfigValue("MyConfigKey");

            Assert.IsNotNull(configValue);
            Assert.AreEqual(configValue,"MyConfigValue");
        }
        [Test]
        public void TryGetValue_ReturnstrueIfFoundInPlane()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.Context = "MyContext";
            string configValue; 
            bool result= sut.TryGetConfigValue("MyConfigKey", out configValue);

            Assert.IsTrue(result);
        }
        [Test]
        public void TryGetValue_OutputsValueIfFoundInPlane()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.Context = "MyContext";
            string configValue;
            bool result = sut.TryGetConfigValue("MyConfigKey", out configValue);

            Assert.IsNotNull(configValue);
            Assert.AreEqual(configValue,"MyConfigValue");
        }
        [Test]
        public void GetConfig_ReturnsNullIfNotFoundInPlane()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.Context = "MyContext";
            string configValue= sut.GetConfigValue("MyConfigUnkownKey");

            Assert.IsNull(configValue);
        }
        [Test]
        public void TryGetValue_ReturnsFalseIfNotFoundInPlane()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.Context = "MyContext";
            string configValue;
            bool result = sut.TryGetConfigValue("MyConfigUnkownKey", out configValue);

            Assert.IsFalse(result);
        }
        [Test]
        public void TryGetValue_OutputsNullIfNotFoundInPlane()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.Context = "MyContext";
            string configValue;
            bool result = sut.TryGetConfigValue("MyConfigUnkownKey", out configValue);

            Assert.IsNull(configValue);
        }

        [Test]
        public void GetValue_PassesToChildNotFoundInPlane()
        {
            var child = MockRepository.GenerateMock<IControlledConfigPlane>();
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");
            sut.Child = child;

           
            sut.Context = "MyContext";
            string configValue = sut.GetConfigValue("MyConfigUnkownKey");

           child.AssertWasCalled(x=>x.GetConfigValue(Arg<string>.Is.Equal("MyConfigUnkownKey")),options=>options.Repeat.Once());

        }
        [Test]
        public void TryGetValue_PassesToChildIfNotFoundInPlane()
        {
            string configValue;

            var child = MockRepository.GenerateMock<IControlledConfigPlane>();
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");
            sut.Child = child;
            
            sut.Context = "MyContext";
            bool result= sut.TryGetConfigValue("MyConfigUnkownKey",out configValue);

            child.AssertWasCalled(x => x.TryGetConfigValue(Arg<string>.Is.Equal("MyConfigUnkownKey"),
                                                           out Arg<string>.Out("hello").Dummy
                                                            ), options => options.Repeat.Once()
                                );
        }

        [Test]
        public void GetConfigKeyReport_ReturnsReportIfValueFound()
        {
            throw new NotImplementedException();

        }
        [Test]
        public void GetConfigKeyReport_ReportsFromChildIfValueNotFound()
        {
            throw new NotImplementedException();

        }

    }
}
