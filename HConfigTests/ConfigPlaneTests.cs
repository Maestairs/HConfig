using System;
using System.Collections.Generic;
using HConfig;
using NUnit.Framework;
using Rhino.Mocks;


namespace HConfigTests
{
    [TestFixture]
    public class ConfigPlaneTests
    {
        [Test]
        public void ChangingLevelNameAfterSetting_CausesException()
        {
            KeyValuePair<string , string> kvp = new KeyValuePair<string, string>("NewName","Fred");
            ConfigPlane sut = new ConfigPlane("FirstName");
            Assert.That(
                            ()=>sut.PlaneDescriptor = kvp,
                            Throws.Exception
                        );
        }
        [Test]
        public void ReadingPlaneDescriptor_ValueReturnedIsEmptyString()
        {
            ConfigPlane sut = new ConfigPlane("FirstName");
            Assert.That(sut.PlaneDescriptor.Value==string.Empty);
        }
        [Test]
        public void ExpectExceptionWhenSettingEmptyDescriptorKey()
        {
            var sut = new ConfigPlane("");
            Assert.That(() =>sut.PlaneDescriptor = new KeyValuePair<string, string>("",""), Throws.Exception);
        }
        [Test]
        public void NewDescriptorIsStoredWhenSpecified()
        {
            var sut = new ConfigPlane("");
            sut.PlaneDescriptor = new KeyValuePair<string, string>("MyPlaneDescriptorKey","SomeValueThatIsIgnored");
            Assert.That(sut.PlaneDescriptor.Key.Equals("MyPlaneDescriptorKey", StringComparison.InvariantCulture));
            Assert.That(sut.PlaneDescriptor.Value.Equals(String.Empty, StringComparison.InvariantCulture));
        }

        [Test]
        public void ExpectExceptionWhenSettingNullDescriptorKey()
        {
            var sut = new ConfigPlane("");
            Assert.That(() => sut.PlaneDescriptor = new KeyValuePair<string, string>(null, ""), Throws.Exception);
        }

        [Test]
        public void AddingNewConfigContext_NewConfigContextStored()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext= new ConfigContext("PlaneName","ConfigContextName");
            sut.UpsertConfigContext(configContext);
            IConfigContext readConfigContext = sut.GetConfigContext("ConfigContextName");
            Assert.IsNotNull(readConfigContext);
            Assert.That(readConfigContext.PlaneDescriptor.Key=="PlaneName");
            Assert.That(readConfigContext.PlaneDescriptor.Value == "ConfigContextName");
        }

        
        [Test]
        public void AddingAConfigContextWithIncorrectPlaneNameThrowsException()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext = new ConfigContext("NotPlaneName", "ConfigContextName");
            Assert.That(()=>sut.UpsertConfigContext(configContext),Throws.ArgumentException);
        }
        
        [Test]
        public void AddingExistingConfigContext_NewConfigContextSaved()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName");
            configContext1.UpsertConfigValue("TestConfig","configContext1Value");
            sut.UpsertConfigContext(configContext1);

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName");
            configContext2.UpsertConfigValue("TestConfig", "configContext2Value");
            sut.UpsertConfigContext(configContext2);

            IConfigContext readConfigContext = sut.GetConfigContext("ConfigContextName");
            Assert.IsNotNull(readConfigContext);
            Assert.That(readConfigContext.PlaneDescriptor.Key == "PlaneName");
            Assert.That(readConfigContext.PlaneDescriptor.Value == "ConfigContextName");

            Assert.That(readConfigContext.GetConfigValue("TestConfig").Equals("configContext2Value",StringComparison.InvariantCulture));
        }
 
        [Test]
        public void GettingConfigContext_ReturnsConfigContextIfPresent()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName2");
            configContext2.UpsertConfigValue("TestConfig", "configContext2Value");
            sut.UpsertConfigContext(configContext2);

            IConfigContext readConfigContext = sut.GetConfigContext("ConfigContextName2");
            Assert.IsNotNull(readConfigContext);
            Assert.That(readConfigContext.PlaneDescriptor.Key == "PlaneName");
            Assert.That(readConfigContext.PlaneDescriptor.Value == "ConfigContextName2");

        }
         
        [Test]
        public void GettingConfigContext_ReturnsNullIfNotPresent()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName2");
            configContext2.UpsertConfigValue("TestConfig", "configContext2Value");
            sut.UpsertConfigContext(configContext2);

            IConfigContext readConfigContext = sut.GetConfigContext("UnknowConfigContextName");
            Assert.IsNull(readConfigContext);
        }
        
        [Test]
        public void TryGetConfigContext_ReturnsTrueIfPresent()
        {
            IConfigContext readConfigContext;
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName2");
            configContext2.UpsertConfigValue("TestConfig", "configContext2Value");
            sut.UpsertConfigContext(configContext2);

            Assert.IsTrue(sut.TryGetConfigContext("ConfigContextName2",out readConfigContext));
            Assert.IsNotNull(readConfigContext);

        }
        
        [Test]
        public void TryGetConfigContext_OutputsValueIfPresent()
        {
            IConfigContext readConfigContext;
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName2");
            configContext2.UpsertConfigValue("TestConfig", "configContext2Value");
            sut.UpsertConfigContext(configContext2);

            Assert.IsTrue(sut.TryGetConfigContext("ConfigContextName2", out readConfigContext));
            Assert.That(readConfigContext.PlaneDescriptor.Value.Equals("ConfigContextName2",StringComparison.InvariantCulture));
        }
        [Test]
        public void TryGetConfigContext_ReturnsFalseIfNotPresent()
        {
            IConfigContext readConfigContext;
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName2");
            configContext2.UpsertConfigValue("TestConfig", "configContext2Value");
            sut.UpsertConfigContext(configContext2);

            Assert.IsFalse(sut.TryGetConfigContext("UnknownConfigContextName", out readConfigContext));
          
        }
        [Test]
        public void TryGetConfigContext_OutputsNullIfNotPresent()
        {
            IConfigContext readConfigContext;
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName2");
            configContext2.UpsertConfigValue("TestConfig", "configContext2Value");
            sut.UpsertConfigContext(configContext2);
            sut.TryGetConfigContext("UnknownConfigContextName", out readConfigContext);
            Assert.IsNull(readConfigContext);
        }
         
        [Test]
        public void TryGetValue_UsesCorrectConfigContextForValue()
        {
            string configValue;
         
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
           
            sut.UpsertConfigContext(configContext1);
            sut.UpsertConfigValue("ConfigContextName1", "TestConfig", "configContext1Value");

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName2");
 
            sut.UpsertConfigContext(configContext2);
            sut.UpsertConfigValue("ConfigContextName2", "TestConfig", "configContext2Value");

            Assert.IsTrue(sut.TryGetConfigValue("ConfigContextName1", "TestConfig", out configValue));
            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("configContext1Value", StringComparison.InvariantCulture));
        }
       
        [Test]
        public void TryGetValue_UsingContextUsesCorrectConfigContextForValue()
        {
            string configValue;

            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName2");
            configContext2.UpsertConfigValue("TestConfig", "configContext2Value");
            sut.UpsertConfigContext(configContext2);
            sut.SearchContext = "ConfigContextName1";
            Assert.IsTrue(sut.TryGetConfigValue("TestConfig", out configValue));

            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("configContext1Value", StringComparison.InvariantCulture));
        }
         
        [Test]
        public void TryGetValue_UsesDefaultValueIfNoValueOnConfigContext()
        {
            string configValue;

            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName2");
            configContext2.UpsertConfigValue("TestConfig", "configContext2Value");
            sut.UpsertConfigContext(configContext2);
            sut.SearchContext = "ConfigContextName1";
            sut.UpsertDefaultConfigValue("ConfigOnlyInDefault","ADefaultValue");

            Assert.IsTrue(sut.TryGetConfigValue("ConfigOnlyInDefault", out configValue));

            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("ADefaultValue", StringComparison.InvariantCulture));
        }
      
        [Test]
        public void GetValue_UsesCorrectConfigContextForValue()
        {
            string configValue;

            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            ConfigContext configContext2 = new ConfigContext("PlaneName", "ConfigContextName2");
            configContext2.UpsertConfigValue("TestConfig", "configContext2Value");
            sut.UpsertConfigContext(configContext2);
            configValue = sut.GetConfigValue("ConfigContextName1", "TestConfig");
    
            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("configContext1Value", StringComparison.InvariantCulture));
        }
        [Test]
        public void GetValue_UsesDefaultIfNotFoundOnConfigContext()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            sut.UpsertDefaultConfigValue("SomeKey","SomeValue");
             
            var configValue = sut.GetConfigValue("ConfigContextName1","SomeKey");

            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("SomeValue", StringComparison.InvariantCulture));
        }
        [Test]
        public void GetValue_UsesContextIfNoConfigContextNameprovided()
        {
            string configValue;

            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            sut.UpsertDefaultConfigValue("SomeKey", "SomeValue");
            sut.SearchContext = "ConfigContextName1";
            configValue = sut.GetConfigValue("TestConfig");

            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("configContext1Value", StringComparison.InvariantCulture));
        }

        [Test]
        public void UpsertingConfigValue_WhereNoConfigContextExists_OneIsCreatedAndConfigStoredOnTheNewConfigContext()
        {
            string configValue;

            ConfigPlane sut = new ConfigPlane("PlaneName");
            
            sut.UpsertConfigValue("MyNewConfigContext","MyKey","MyValue");


            configValue = sut.GetConfigValue("MyNewConfigContext", "MyKey");
            Assert.IsNotNull(sut.GetConfigContext("MyNewConfigContext"));
            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("MyValue", StringComparison.InvariantCulture));
        }
         
        [Test]
        public void UpsertingConfigValue_WhereNoContextConfigContextExists_OneIsCreatedAndConfigStoredOnTheNewConfigContext()
        {
            string configValue;

            ConfigPlane sut = new ConfigPlane("PlaneName");
            sut.SearchContext = "MyNewConfigContext";
            sut.UpsertConfigValue( "MyKey", "MyValue");


            configValue = sut.GetConfigValue("MyNewConfigContext", "MyKey");
            Assert.IsNotNull(sut.GetConfigContext("MyNewConfigContext"));
            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("MyValue", StringComparison.InvariantCulture));
        }

        [Test]
        public void GetConfigKeyReport_ReturnsNullForUnknownKey()
        {
             

            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("TestConfig", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            sut.UpsertDefaultConfigValue("SomeKey", "SomeValue");
            sut.SearchContext = "ConfigContextName1";
            var configKeyReport = sut.GetConfigKeyReport("UnknownTestConfig");

            Assert.IsNull(configKeyReport);
           
        }

        [Test]
        public void SettingContextToAnUnusedContext_GetConfigValueWillReturnValueFromDefault()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigContext configContext1 = new ConfigContext("PlaneName", "ConfigContextName1");
            configContext1.UpsertConfigValue("SomeKey", "configContext1Value");
            sut.UpsertConfigContext(configContext1);

            sut.UpsertDefaultConfigValue("SomeKey", "SomeValue");
            sut.SearchContext = "Invalid";
            var configValue = sut.GetConfigValue( "SomeKey");

            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("SomeValue", StringComparison.InvariantCulture));

        }




        [Test]
        public void NewConfigPlaneHasNullChild()
        {
            ConfigPlane sut = new ConfigPlane("MyPlane");
            Assert.IsNull(sut.Child);
        }



        [Test]
        public void GetConfig_ReturnsValueIfKeyFound()
        {
            ConfigPlane sut = new ConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue = sut.GetConfigValue("MyConfigKey");

            Assert.IsNotNull(configValue);
            Assert.AreEqual(configValue, "MyConfigValue");
        }
        [Test]
        public void TryGetValue_ReturnstrueIfKeyFound()
        {
            ConfigPlane sut = new ConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue;
            bool result = sut.TryGetConfigValue("MyConfigKey", out configValue);

            Assert.IsTrue(result);
        }
        [Test]
        public void TryGetValue_OutputsValueIfKeyFound()
        {
            ConfigPlane sut = new ConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue;
            sut.TryGetConfigValue("MyConfigKey", out configValue);

            Assert.IsNotNull(configValue);
            Assert.AreEqual(configValue, "MyConfigValue");
        }
        [Test]
        public void GetConfig_ReturnsNullIfKeyNotFound()
        {
            ConfigPlane sut = new ConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue = sut.GetConfigValue("MyConfigUnkownKey");

            Assert.IsNull(configValue);
        }
        [Test]
        public void TryGetValue_ReturnsFalseIfKeyNotFound()
        {
            ConfigPlane sut = new ConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue;
            bool result = sut.TryGetConfigValue("MyConfigUnkownKey", out configValue);

            Assert.IsFalse(result);
        }
        [Test]
        public void TryGetValue_OutputsNullIfKeyNotFound()
        {
            ConfigPlane sut = new ConfigPlane("MyPlane");

            sut.UpsertConfigValue("MyContext", "MyConfigKey", "MyConfigValue");
            sut.SearchContext = "MyContext";
            string configValue;
            sut.TryGetConfigValue("MyConfigUnkownKey", out configValue);

            Assert.IsNull(configValue);
        }

        [Test]
        public void GetValue_AttemptsToGetValueFromChildIfNotFoundLocally()
        {
            var child = MockRepository.GenerateMock<IConfigPlane>();
            ConfigPlane sut = new ConfigPlane("MyPlane");
            sut.Child = child;


            sut.SearchContext = "MyContext";
            sut.GetConfigValue("MyConfigUnkownKey");

            child.AssertWasCalled(x => x.GetConfigValue(Arg<string>.Is.Equal("MyConfigUnkownKey")), options => options.Repeat.Once());

        }
        [Test]
        public void TryGetValue_PassesToChildIfKeyNotFound()
        {
            string configValue;

            var child = MockRepository.GenerateMock<IConfigPlane>();
            ConfigPlane sut = new ConfigPlane("MyPlane");
            sut.Child = child;

            sut.SearchContext = "MyContext";
            sut.TryGetConfigValue("MyConfigUnkownKey", out configValue);

            child.AssertWasCalled(x => x.TryGetConfigValue(Arg<string>.Is.Equal("MyConfigUnkownKey"),
                                                           out Arg<string>.Out("hello").Dummy
                                                            ), options => options.Repeat.Once()
                                );
        }

        [Test]
        public void UsingInvalidContext_CausesException()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
           
            Assert.That(()=>sut.UpsertConfigValue("MyKey", "MyValue"),Throws.ArgumentException);
        }

        [Test]
        public void GetConfigKeyReport_ReturnsReportIfKeyFound()
        {

            ConfigPlane sut = new ConfigPlane("PlaneName");
            sut.SearchContext = "SomeContext";
            sut.UpsertConfigValue("SomeContext","MyKey", "MyValue");
            var configKeyReport = sut.GetConfigKeyReport("MyKey");
            Assert.NotNull(configKeyReport);
        }


        [Test]
        public void GetConfigKeyReport_ReportsFromChildIfKeyNotFound()
        {
            ConfigPlane sut = new ConfigPlane("TopPlane");
            ConfigPlane child = new ConfigPlane("ChildPlane");
            child.SearchContext = "SomeContext";
            child.UpsertConfigValue("SomeContext", "MyKey", "MyValue");
            

            sut.Child = child;
            sut.SearchContext = "SomeSearchContext";
            var configKeyReport = sut.GetConfigKeyReport("MyKey");


            Assert.NotNull(configKeyReport);

        }

        [Test]
        public void GetConfigKeyReport_ReturnsCorrectValuesForAConfigSourceOnAConfigContext()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            sut.SearchContext = "SomeContext";
            sut.UpsertConfigValue("SomeContext", "MyKey", "MyValue");
            var configKeyReport = sut.GetConfigKeyReport("MyKey");
            Assert.That(configKeyReport.ConfigSource == ConfigKeySource.ContextSpecific);
        }
        [Test]
        public void GetConfigKeyReport_ReturnsCorrectValuesForADefaultConfigSource()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            sut.SearchContext = "SomeContext";
            sut.UpsertDefaultConfigValue( "MyKey", "MyValue");
            var configKeyReport = sut.GetConfigKeyReport("MyKey");
            Assert.That(configKeyReport.ConfigSource == ConfigKeySource.Default);
        }
        [Test]
        public void GetConfigKeyReport_ReturnsCorrectConfigPlaneName()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            sut.SearchContext = "SomeContext";
            sut.UpsertConfigValue("SomeContext", "MyKey", "MyValue");
            var configKeyReport = sut.GetConfigKeyReport("MyKey");
            Assert.That(configKeyReport.PlaneName == "PlaneName");
        }
        [Test]
        public void GetConfigKeyReport_ReturnsCorrectConfigKeyName()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            sut.SearchContext = "SomeContext";
            sut.UpsertConfigValue("SomeContext", "MyKey", "MyValue");
            var configKeyReport = sut.GetConfigKeyReport("MyKey");
            Assert.That(configKeyReport.Key == "MyKey");
        }
        [Test]
        public void GetConfigKeyReport_ReturnsCorrectConfigValue()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            sut.SearchContext = "SomeContext";
            sut.UpsertConfigValue("SomeContext", "MyKey", "MyValue");
            var configKeyReport = sut.GetConfigKeyReport("MyKey");
            Assert.That(configKeyReport.Value == "MyValue");
        }
        [Test]
        public void GetConfigKeyReport_ReturnsCorrectConfigContextame()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            sut.SearchContext = "SomeContext";
            sut.UpsertConfigValue("SomeContext", "MyKey", "MyValue");
            var configKeyReport = sut.GetConfigKeyReport("MyKey");
            Assert.That(configKeyReport.ConfigContextName == "SomeContext");
        }
    }
}
