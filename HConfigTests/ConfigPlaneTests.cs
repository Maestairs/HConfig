using System;
using System.Collections.Generic;
using HConfig;
using NUnit.Framework;



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
        public void AddingNewSpoke_NewSpokeStored()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke= new ConfigSpoke("PlaneName","SpokeName");
            sut.UpsertSpoke(spoke);
            IConfigSpoke readSpoke = sut.GetSpoke("SpokeName");
            Assert.IsNotNull(readSpoke);
            Assert.That(readSpoke.PlaneDescriptor.Key=="PlaneName");
            Assert.That(readSpoke.PlaneDescriptor.Value == "SpokeName");
        }

        
        [Test]
        public void AddingASpokeWithIncorrectPlaneNameThrowsException()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke = new ConfigSpoke("NotPlaneName", "SpokeName");
            Assert.That(()=>sut.UpsertSpoke(spoke),Throws.ArgumentException);
        }
        
        [Test]
        public void AddingExistingSpoke_NewSpokeSaved()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName");
            spoke1.UpsertConfigValue("TestConfig","spoke1Value");
            sut.UpsertSpoke(spoke1);

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName");
            spoke2.UpsertConfigValue("TestConfig", "spoke2Value");
            sut.UpsertSpoke(spoke2);

            IConfigSpoke readSpoke = sut.GetSpoke("SpokeName");
            Assert.IsNotNull(readSpoke);
            Assert.That(readSpoke.PlaneDescriptor.Key == "PlaneName");
            Assert.That(readSpoke.PlaneDescriptor.Value == "SpokeName");

            Assert.That(readSpoke.GetConfigValue("TestConfig").Equals("spoke2Value",StringComparison.InvariantCulture));
        }
 
        [Test]
        public void GettingSpoke_ReturnsSpokeIfPresent()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName1");
            spoke1.UpsertConfigValue("TestConfig", "spoke1Value");
            sut.UpsertSpoke(spoke1);

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName2");
            spoke2.UpsertConfigValue("TestConfig", "spoke2Value");
            sut.UpsertSpoke(spoke2);

            IConfigSpoke readSpoke = sut.GetSpoke("SpokeName2");
            Assert.IsNotNull(readSpoke);
            Assert.That(readSpoke.PlaneDescriptor.Key == "PlaneName");
            Assert.That(readSpoke.PlaneDescriptor.Value == "SpokeName2");

        }
         
        [Test]
        public void GettingSpoke_ReturnsNullIfNotPresent()
        {
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName1");
            spoke1.UpsertConfigValue("TestConfig", "spoke1Value");
            sut.UpsertSpoke(spoke1);

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName2");
            spoke2.UpsertConfigValue("TestConfig", "spoke2Value");
            sut.UpsertSpoke(spoke2);

            IConfigSpoke readSpoke = sut.GetSpoke("UnknowSpokeName");
            Assert.IsNull(readSpoke);
        }
        
        [Test]
        public void TryGetSpoke_ReturnsTrueIfPresent()
        {
            IConfigSpoke readSpoke;
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName1");
            spoke1.UpsertConfigValue("TestConfig", "spoke1Value");
            sut.UpsertSpoke(spoke1);

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName2");
            spoke2.UpsertConfigValue("TestConfig", "spoke2Value");
            sut.UpsertSpoke(spoke2);

            Assert.IsTrue(sut.TryGetSpoke("SpokeName2",out readSpoke));
            Assert.IsNotNull(readSpoke);

        }
        
        [Test]
        public void TryGetSpoke_OutputsValueIfPresent()
        {
            IConfigSpoke readSpoke;
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName1");
            spoke1.UpsertConfigValue("TestConfig", "spoke1Value");
            sut.UpsertSpoke(spoke1);

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName2");
            spoke2.UpsertConfigValue("TestConfig", "spoke2Value");
            sut.UpsertSpoke(spoke2);

            Assert.IsTrue(sut.TryGetSpoke("SpokeName2", out readSpoke));
            Assert.That(readSpoke.PlaneDescriptor.Value.Equals("SpokeName2",StringComparison.InvariantCulture));
        }
        [Test]
        public void TryGetSpoke_ReturnsFalseIfNotPresent()
        {
            IConfigSpoke readSpoke;
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName1");
            spoke1.UpsertConfigValue("TestConfig", "spoke1Value");
            sut.UpsertSpoke(spoke1);

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName2");
            spoke2.UpsertConfigValue("TestConfig", "spoke2Value");
            sut.UpsertSpoke(spoke2);

            Assert.IsFalse(sut.TryGetSpoke("UnknownSpokeName", out readSpoke));
          
        }
        [Test]
        public void TryGetSpoke_OutputsNullIfNotPresent()
        {
            IConfigSpoke readSpoke;
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName1");
            spoke1.UpsertConfigValue("TestConfig", "spoke1Value");
            sut.UpsertSpoke(spoke1);

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName2");
            spoke2.UpsertConfigValue("TestConfig", "spoke2Value");
            sut.UpsertSpoke(spoke2);
            sut.TryGetSpoke("UnknownSpokeName", out readSpoke);
            Assert.IsNull(readSpoke);
        }
         
        [Test]
        public void TryGetValue_UsesCorrectSpokeForValue()
        {
            string configValue;
         
            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName1");
           
            sut.UpsertSpoke(spoke1);
            sut.UpsertConfigValue("SpokeName1", "TestConfig", "spoke1Value");

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName2");
 
            sut.UpsertSpoke(spoke2);
            sut.UpsertConfigValue("SpokeName2", "TestConfig", "spoke2Value");

            Assert.IsTrue(sut.TryGetConfigValue("SpokeName1", "TestConfig", out configValue));
            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("spoke1Value", StringComparison.InvariantCulture));
        }
       
        [Test]
        public void TryGetValue_UsingContextUsesCorrectSpokeForValue()
        {
            string configValue;

            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName1");
            spoke1.UpsertConfigValue("TestConfig", "spoke1Value");
            sut.UpsertSpoke(spoke1);

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName2");
            spoke2.UpsertConfigValue("TestConfig", "spoke2Value");
            sut.UpsertSpoke(spoke2);
            sut.Context = "SpokeName1";
            Assert.IsTrue(sut.TryGetConfigValue("TestConfig", out configValue));

            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("spoke1Value", StringComparison.InvariantCulture));
        }
         
        [Test]
        public void TryGetValue_UsesDefaultValueIfNoValueOnSpoke()
        {
            string configValue;

            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName1");
            spoke1.UpsertConfigValue("TestConfig", "spoke1Value");
            sut.UpsertSpoke(spoke1);

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName2");
            spoke2.UpsertConfigValue("TestConfig", "spoke2Value");
            sut.UpsertSpoke(spoke2);
            sut.Context = "SpokeName1";
            sut.UpsertDefaultConfigValue("ConfigOnlyInDefault","ADefaultValue");

            Assert.IsTrue(sut.TryGetConfigValue("ConfigOnlyInDefault", out configValue));

            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("ADefaultValue", StringComparison.InvariantCulture));
        }
      
        [Test]
        public void GetValue_UsesCorrectSpokeForValue()
        {
            string configValue;

            ConfigPlane sut = new ConfigPlane("PlaneName");
            ConfigSpoke spoke1 = new ConfigSpoke("PlaneName", "SpokeName1");
            spoke1.UpsertConfigValue("TestConfig", "spoke1Value");
            sut.UpsertSpoke(spoke1);

            ConfigSpoke spoke2 = new ConfigSpoke("PlaneName", "SpokeName2");
            spoke2.UpsertConfigValue("TestConfig", "spoke2Value");
            sut.UpsertSpoke(spoke2);
            configValue = sut.GetConfigValue("SpokeName1", "TestConfig");
    
            Assert.IsNotNull(configValue);
            Assert.That(configValue.Equals("spoke1Value", StringComparison.InvariantCulture));
        }
        /*
        [Test]
        public void UpsertingConfigValue_WhereNoSpokeExists_OneIsCreatedAndConfigStoredOnTheNewSpoke()
        {
        }
        [Test]
        public void UpsertingConfigValue_WhereNoContextSpokeExists_OneIsCreatedAndConfigStoredOnTheNewSpoke()
        {
        }
        [Test]
        public void UsingInvalidContext_CausesException()
        {
        }
        [Test]
        public void UpsertConfigValue_WritesToCorrectSpoke()
        {
            
        }
        [Test]
        public void UpsertConfigValue_WritesToCorrectSpokeWhenUsingContext()
        {

        }
        
        [Test]
        public void GetValue_FallsBackToHubIfNoValueOnSpoke()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void GetValue_UsingContextUsesCorrectSpokeForValue()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void GetValue_UsingContextFallsBackToHubIfNoValueOnSpoke()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GetValue_UsingContextThrowsExceptionIfNoContextSet()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetValue_UsingContextThrowsExceptionIfNoContextSet()
        {
            throw new NotImplementedException();
        }



        [Test]
        public void UpsertingNewNameAndValue_CausesValueToBeStored()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void UpsertingExistingNameAndValue_CausesValueToBeStored()
        {
            throw new NotImplementedException();

        }

        [Test]
        public void TryGetValue_ReturnsFalseIfNoFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TryGetValue_ReturnsTrueIfFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void TryGetValue_SetsValueIfFound()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void TryGetValue_ReturnsCorrectValueWhenMultiopleEntries()
        {
            throw new NotImplementedException();
        }


        [Test]
        public void GetValue_ReturnsNullIfNotFound()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GetValue_ReturnsValueIfFound()
        {
            throw new NotImplementedException();
        }
        [Test]
        public void GetValue_ReturnsCorrectValueWhenMultiopleEntries()
        {
            throw new NotImplementedException();
        }
        */
    }
}
