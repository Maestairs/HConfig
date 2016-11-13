using System;
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
        public void GetConfig_ReturnsValueIfKeyFound()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue = sut.GetConfigValue("MyConfigKey");

            Assert.IsNotNull(configValue);
            Assert.AreEqual(configValue,"MyConfigValue");
        }
        [Test]
        public void TryGetValue_ReturnstrueIfKeyFound()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue; 
            bool result= sut.TryGetConfigValue("MyConfigKey", out configValue);

            Assert.IsTrue(result);
        }
        [Test]
        public void TryGetValue_OutputsValueIfKeyFound()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue;
            sut.TryGetConfigValue("MyConfigKey", out configValue);

            Assert.IsNotNull(configValue);
            Assert.AreEqual(configValue,"MyConfigValue");
        }
        [Test]
        public void GetConfig_ReturnsNullIfKeyNotFound()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue= sut.GetConfigValue("MyConfigUnkownKey");

            Assert.IsNull(configValue);
        }
        [Test]
        public void TryGetValue_ReturnsFalseIfKeyNotFound()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue;
            bool result = sut.TryGetConfigValue("MyConfigUnkownKey", out configValue);

            Assert.IsFalse(result);
        }
        [Test]
        public void TryGetValue_OutputsNullIfKeyNotFound()
        {
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue;
            sut.TryGetConfigValue("MyConfigUnkownKey", out configValue);

            Assert.IsNull(configValue);
        }

        [Test]
        public void GetValue_PassesToChildKeyNotFound()
        {
            var child = MockRepository.GenerateMock<IControlledConfigPlane>();
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");
            sut.Child = child;

           
            sut.SearchContext = "MyContext";
            sut.GetConfigValue("MyConfigUnkownKey");

           child.AssertWasCalled(x=>x.GetConfigValue(Arg<string>.Is.Equal("MyConfigUnkownKey")),options=>options.Repeat.Once());

        }
        [Test]
        public void TryGetValue_PassesToChildIfKeyNotFound()
        {
            string configValue;

            var child = MockRepository.GenerateMock<IControlledConfigPlane>();
            ControlledConfigPlane sut = new ControlledConfigPlane("MyPlane");
            sut.Child = child;
            
            sut.SearchContext = "MyContext";
            sut.TryGetConfigValue("MyConfigUnkownKey",out configValue);

            child.AssertWasCalled(x => x.TryGetConfigValue(Arg<string>.Is.Equal("MyConfigUnkownKey"),
                                                           out Arg<string>.Out("hello").Dummy
                                                            ), options => options.Repeat.Once()
                                );
        }

        [Test]
        public void GetConfigKeyReport_ReturnsReportIfKeyFound()
        {
            throw new NotImplementedException();

        }
        [Test]
        public void GetConfigKeyReport_ReportsFromChildIfKeyNotFound()
        {
            throw new NotImplementedException();

        }

    }
}
