using System;
using System.Collections.Generic;
using HConfig;
using NUnit.Framework;


namespace HConfigTests
{
    [TestFixture]
    public class ConfigControllerTests
    {
        [Test]
        public void OnCreate_PriorityIsNull()
        {
            ConfigController sut = new ConfigController();
            Assert.IsNull(sut.Priority);
        }
        [Test]
        public void OnCreate_ContextIsNull()
        {
            ConfigController sut = new ConfigController();
            Assert.IsNull(sut.SearchContext);
        }

        [TestCase("FirstPlane", "SecondPlane", "ThirdPlane", "FourthPlane")]
        [TestCase("SecondPlane", "FirstPlane", "ThirdPlane", "FourthPlane")]
        [TestCase("FourthPlane", "FirstPlane", "ThirdPlane", "SecondPlane")]
        public void PrioritisePlanes_SetsOrderAsExpected(string firstPlane, string secondPlane, string thirdPlane,
            string fourthPlane)
        {
            ConfigControllerTestHelper sut = new ConfigControllerTestHelper();
            sut.UpsertConfigValue("FirstPlane","MyConfigContext","ConfigKey","ConfigValueFirstPlane");
            sut.UpsertConfigValue("SecondPlane", "MyConfigContext", "ConfigKey", "ConfigValueSecondPlane");
            sut.UpsertConfigValue("ThirdPlane", "MyConfigContext", "ConfigKey", "ConfigValueThirdPlane");
            sut.UpsertConfigValue("FourthPlane", "MyConfigContext", "ConfigKey", "ConfigValueFourthPlane");

            Queue<string>priority = new Queue<string>();
             
            priority.Enqueue(firstPlane);
            priority.Enqueue(secondPlane);
            priority.Enqueue(thirdPlane);
            priority.Enqueue(fourthPlane);

            sut.Priority = priority;

            IControlledConfigPlane planePointer = sut.GetEntryPoint();

            Assert.That(planePointer.PlaneDescriptor.Key.Equals(firstPlane,StringComparison.InvariantCulture));
            Assert.That(planePointer.Child.PlaneDescriptor.Key.Equals(secondPlane,StringComparison.InvariantCulture));
            Assert.That(planePointer.Child.Child.PlaneDescriptor.Key.Equals(thirdPlane, StringComparison.InvariantCulture));
            Assert.That(planePointer.Child.Child.Child.PlaneDescriptor.Key.Equals(fourthPlane, StringComparison.InvariantCulture));
            Assert.IsNull(planePointer.Child.Child.Child.Child);
        }
        [TestCase("FirstPlane", "SecondPlane", "ThirdPlane", "FourthPlane")]
        [TestCase("SecondPlane", "FirstPlane", "ThirdPlane", "FourthPlane")]
        [TestCase("FourthPlane", "FirstPlane", "ThirdPlane", "SecondPlane")]
        public void PrioritisePlanes_SetsOrderAsExpectedIfSetBeforePlanesAdded(string firstPlane, string secondPlane, string thirdPlane,
            string fourthPlane)
        {
            ConfigControllerTestHelper sut = new ConfigControllerTestHelper();

            Queue<string> priority = new Queue<string>();
            priority.Enqueue(firstPlane);
            priority.Enqueue(secondPlane);
            priority.Enqueue(thirdPlane);
            priority.Enqueue(fourthPlane);

            sut.Priority = priority;

            sut.UpsertConfigValue("FirstPlane", "MyConfigContext", "ConfigKey", "ConfigValueFirstPlane");
            sut.UpsertConfigValue("SecondPlane", "MyConfigContext", "ConfigKey", "ConfigValueSecondPlane");
            sut.UpsertConfigValue("ThirdPlane", "MyConfigContext", "ConfigKey", "ConfigValueThirdPlane");
            sut.UpsertConfigValue("FourthPlane", "MyConfigContext", "ConfigKey", "ConfigValueFourthPlane");



            IControlledConfigPlane planePointer = sut.GetEntryPoint();

            Assert.That(planePointer.PlaneDescriptor.Key.Equals(firstPlane, StringComparison.InvariantCulture));
            Assert.That(planePointer.Child.PlaneDescriptor.Key.Equals(secondPlane, StringComparison.InvariantCulture));
            Assert.That(planePointer.Child.Child.PlaneDescriptor.Key.Equals(thirdPlane, StringComparison.InvariantCulture));
            Assert.That(planePointer.Child.Child.Child.PlaneDescriptor.Key.Equals(fourthPlane, StringComparison.InvariantCulture));
            Assert.IsNull(planePointer.Child.Child.Child.Child);
        }
 


        [Test]
        public void WithSinglePlaneUpsertConfigValue_GetValueIsReadCorrectly()
        {
            ConfigControllerTestHelper sut = new ConfigControllerTestHelper();

            Queue<string> priority = new Queue<string>();
            priority.Enqueue("FirstPlane");


            sut.Priority = priority;

            sut.UpsertConfigValue("FirstPlane", "MyConfigContext", "ConfigKey", "ConfigValue");
            Dictionary<string, string> context= new Dictionary<string, string>();
            context.Add("FirstPlane","MyConfigContext");
            sut.SearchContext = context;

            string result = sut.GetConfigValue("ConfigKey");
            Assert.NotNull(result);
            Assert.That(result.Equals("ConfigValue"));


        }
        [Test]
        public void WithSinglePlaneUpsertConfigValue_TryGetValueIsReadCorrectly()
        {
            ConfigControllerTestHelper sut = new ConfigControllerTestHelper();

            Queue<string> priority = new Queue<string>();
            priority.Enqueue("FirstPlane");


            sut.Priority = priority;

            sut.UpsertConfigValue("FirstPlane", "MyConfigContext", "ConfigKey", "ConfigValue");
            Dictionary<string, string> context = new Dictionary<string, string>();
            context.Add("FirstPlane", "MyConfigContext");
            sut.SearchContext = context;

            string configValue;
            bool success = sut.TryGetConfigValue("ConfigKey",out configValue);
            Assert.NotNull(configValue);
            Assert.That(configValue.Equals("ConfigValue"));
            Assert.IsTrue(success);
        }


        [Test]
        public void WithSinglePlaneUpsertDefaultConfigValue_GetValueIsReadCorrectly()
        {
            ConfigControllerTestHelper sut = new ConfigControllerTestHelper();

            Queue<string> priority = new Queue<string>();
            priority.Enqueue("FirstPlane");


            sut.Priority = priority;

            sut.UpsertDefaultConfigValue("FirstPlane", "ConfigKey", "ConfigValue");
            Dictionary<string, string> context = new Dictionary<string, string>();
            context.Add("FirstPlane", "MyConfigContext");
            sut.SearchContext = context;

            string result = sut.GetConfigValue("ConfigKey");
            Assert.NotNull(result);
            Assert.That(result.Equals("ConfigValue"));


        }
        [Test]
        public void WithSinglePlaneUpsertDefaultConfigValue_TryGetValueIsReadCorrectly()
        {
            ConfigControllerTestHelper sut = new ConfigControllerTestHelper();

            Queue<string> priority = new Queue<string>();
            priority.Enqueue("FirstPlane");


            sut.Priority = priority;

            sut.UpsertDefaultConfigValue("FirstPlane",  "ConfigKey", "ConfigValue");
            Dictionary<string, string> context = new Dictionary<string, string>();
            context.Add("FirstPlane", "MyConfigContext");
            sut.SearchContext = context;

            string configValue;
            bool success = sut.TryGetConfigValue("ConfigKey", out configValue);
            Assert.NotNull(configValue);
            Assert.That(configValue.Equals("ConfigValue"));
            Assert.IsTrue(success);
        }

        [TestCase("FirstPlane", "SecondPlane", "ThirdPlane", "FourthPlane", "ConfigValueFirstPlane")]
        [TestCase("SecondPlane", "FirstPlane", "ThirdPlane", "FourthPlane", "ConfigValueSecondPlane")]
        [TestCase("FourthPlane", "FirstPlane", "ThirdPlane", "SecondPlane", "ConfigValueFourthPlane")]
        public void WithMultiplePlanes_The_Value_IsReadAsExpectedByPriority(string firstPlane, string secondPlane, string thirdPlane,
            string fourthPlane, string expectedValue)
        {
            ConfigController sut = new ConfigController();
            sut.UpsertConfigValue("FirstPlane", "MyConfigContext", "ConfigKey", "ConfigValueFirstPlane");
            sut.UpsertConfigValue("SecondPlane", "MyConfigContext", "ConfigKey", "ConfigValueSecondPlane");
            sut.UpsertConfigValue("ThirdPlane", "MyConfigContext", "ConfigKey", "ConfigValueThirdPlane");
            sut.UpsertConfigValue("FourthPlane", "MyConfigContext", "ConfigKey", "ConfigValueFourthPlane");


            Queue<string> priority = new Queue<string>();
            priority.Enqueue(firstPlane);
            priority.Enqueue(secondPlane);
            priority.Enqueue(thirdPlane);
            priority.Enqueue(fourthPlane);

            sut.Priority = priority;
            Dictionary<string, string> context = new Dictionary<string, string>();
            context.Add("FirstPlane", "MyConfigContext");
            context.Add("SecondPlane", "MyConfigContext");
            context.Add("ThirdPlane", "MyConfigContext");
            context.Add("FourthPlane", "MyConfigContext");
            sut.SearchContext = context;

            string configValue = sut.GetConfigValue("ConfigKey");
            Assert.That(configValue.Equals(expectedValue,StringComparison.InvariantCulture));
        }

        [TestCase("FirstPlane", "SecondPlane", "ThirdPlane", "FourthPlane", "ConfigValueFirstPlane")]
        [TestCase("SecondPlane", "FirstPlane", "ThirdPlane", "FourthPlane", "ConfigValueSecondPlane")]
        [TestCase("FourthPlane", "FirstPlane", "ThirdPlane", "SecondPlane", "ConfigValueFourthPlane")]
        public void WithMultiplePlanes_The_DefaultValue_IsReadAsExpectedByPriority(string firstPlane, string secondPlane, string thirdPlane,
            string fourthPlane, string expectedValue)
        {
            ConfigController sut = new ConfigController();
            sut.UpsertDefaultConfigValue("FirstPlane", "ConfigKey", "ConfigValueFirstPlane");
            sut.UpsertDefaultConfigValue("SecondPlane", "ConfigKey", "ConfigValueSecondPlane");
            sut.UpsertDefaultConfigValue("ThirdPlane", "ConfigKey", "ConfigValueThirdPlane");
            sut.UpsertDefaultConfigValue("FourthPlane", "ConfigKey", "ConfigValueFourthPlane");


            Queue<string> priority = new Queue<string>();
            priority.Enqueue(firstPlane);
            priority.Enqueue(secondPlane);
            priority.Enqueue(thirdPlane);
            priority.Enqueue(fourthPlane);

            sut.Priority = priority;
            Dictionary<string, string> context = new Dictionary<string, string>();
            context.Add("FirstPlane", "MyConfigContext");
            context.Add("SecondPlane", "MyConfigContext");
            context.Add("ThirdPlane", "MyConfigContext");
            context.Add("FourthPlane", "MyConfigContext");
            sut.SearchContext = context;

            string configValue = sut.GetConfigValue("ConfigKey");
            Assert.That(configValue.Equals(expectedValue, StringComparison.InvariantCulture));
        }

        [Test]
        public void GetConfigKeyReport_ReturnsNullIfConfigNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GetConfigKeyReport_ReturnsReportIfConfigValueFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void AddingAConfigValue_KeyAddedToKnownKeys()
        {

            throw new NotImplementedException();
        }
        [Test]
        public void AddingAknownConfigValue_KeyNotAddedToKnownKeys()
        {
            throw new NotImplementedException();

        }

        [Test]
        public void GetConfigKeyReports_ReturnsValidListOfContextKeyReports()
        {
            throw new NotImplementedException();
        }
    }

    internal class ConfigControllerTestHelper : ConfigController
    {
        internal IControlledConfigPlane GetEntryPoint()
        {
            return _entryPoint;
        }
    }
    
}
