using System;
using System.Collections.Generic;
using HConfig;
using NUnit.Framework;


namespace HConfigTests
{
    [TestFixture]
    public class ConfigContextTests
    {
        [Test]
        public void ChangingLevelNameAfterSetting_CausesException()
        {
            ConfigContext sut= new ConfigContext("Key","Value");
            var keyValuePair = new KeyValuePair<string, string>("newkey","Value");
            Assert.That(
                            ()=>sut.PlaneDescriptor=keyValuePair,
                            Throws.Exception
                        );
        }
        [Test]
        public void ChangingLevelValueAfterSetting_CausesException()
        {

            ConfigContext sut = new ConfigContext("Key", "Value");
            var keyValuePair = new KeyValuePair<string, string>("Key", "newValue");
            Assert.That(
                            () => sut.PlaneDescriptor = keyValuePair,
                            Throws.Exception
                        );
        }

        [Test]
        public void SettingNullDescriptorKey_ThrowsException()
        {
            ConfigContext  sut = new ConfigContext("","");
            Assert.That(()=>sut.PlaneDescriptor = new KeyValuePair<string, string>(null,"ss"),Throws.Exception);
        }
        [Test]
        public void SettingEmptyDescriptorKey_ThrowsException()
        {
            ConfigContext sut = new ConfigContext("", "");
            Assert.That(() => sut.PlaneDescriptor = new KeyValuePair<string, string>("", "ss"), Throws.Exception);
        }
        [Test]
        public void SettingNullDescriptorValue_ThrowsException()
        {
            ConfigContext sut = new ConfigContext("", "");
            Assert.That(() => sut.PlaneDescriptor = new KeyValuePair<string, string>("ss", null), Throws.Exception);
        }
        [Test]
        public void SettingEmptyDescriptorValue_ThrowsException()
        {
            ConfigContext sut = new ConfigContext("", "");
            Assert.That(() => sut.PlaneDescriptor = new KeyValuePair<string, string>("ss", ""), Throws.Exception);
        }
        [Test]
        public void SettingValidDescriptorValue_ValueIsStored()
        {
            ConfigContext sut = new ConfigContext("", "");
            sut.PlaneDescriptor = new KeyValuePair<string, string>("PlaneName", "ConfigContextName");
            Assert.That(sut.PlaneDescriptor.Key.Equals("PlaneName", StringComparison.InvariantCulture));
            Assert.That(sut.PlaneDescriptor.Value.Equals("ConfigContextName", StringComparison.InvariantCulture));
        }

        [Test]
        public void UpsertingNewNameAndValue_CausesValueToBeStored()
        {
            ConfigContext sut = new ConfigContext("LevelKey", "LevelValue");
            sut.UpsertConfigValue("AConfigKey","AConfigValue");
            
            Assert.That(sut.GetConfigValue("AConfigKey").Equals("AConfigValue",StringComparison.InvariantCulture));
        }
        [Test]
        public void UpsertingExistingNameAndValue_CausesValueToBeStored()
        {
            ConfigContext sut = new ConfigContext("LevelKey", "LevelValue");
            sut.UpsertConfigValue("AConfigKey", "AConfigValue");
            sut.UpsertConfigValue("AConfigKey", "ANewConfigValue");
            Assert.That(sut.GetConfigValue("AConfigKey").Equals("ANewConfigValue", StringComparison.InvariantCulture));

        }
    
        [Test]
        public void TryGetValue_ReturnsFalseIfNoFound()
        {
            string configValue;
            ConfigContext sut = new ConfigContext("LevelKey", "LevelValue");
            sut.UpsertConfigValue("AConfigKey", "AConfigValue");
            Assert.IsFalse(sut.TryGetConfigValue("MissingKey", out configValue));
        }
        [Test]
        public void TryGetValue_ReturnsTrueIfFound()
        {
            string configValue;
            ConfigContext sut = new ConfigContext("LevelKey", "LevelValue");
            sut.UpsertConfigValue("AConfigKey", "AConfigValue");
            Assert.IsTrue(sut.TryGetConfigValue("AConfigKey", out configValue));
        }
    
        [Test]
        public void TryGetValue_SetsValueIfFound()
        {
            string configValue;
            ConfigContext sut = new ConfigContext("LevelKey", "LevelValue");
            sut.UpsertConfigValue("AConfigKey", "AConfigValue");
            sut.TryGetConfigValue("AConfigKey", out configValue);
            Assert.That(configValue.Equals("AConfigValue",StringComparison.InvariantCulture));
        }
        [Test]
        public void TryGetValue_ReturnsCorrectValueWhenMultiopleEntries()
        {
            string configValue;
            ConfigContext sut = new ConfigContext("LevelKey", "LevelValue");
            sut.UpsertConfigValue("AKey1", "AValue1");
            sut.UpsertConfigValue("AKey2", "AValue2");
            sut.UpsertConfigValue("AKey3", "AValue3");
            sut.UpsertConfigValue("AKey4", "AValue4");
            sut.UpsertConfigValue("AKey5", "AValue5");
            sut.UpsertConfigValue("AKey6", "AValue6");
            sut.UpsertConfigValue("AKey7", "AValue7");
            sut.UpsertConfigValue("AKey8", "AValue8");
            sut.UpsertConfigValue("AKey9", "AValue9");
            sut.UpsertConfigValue("AKey10", "AValue10");
            sut.UpsertConfigValue("AKey11", "AValue11");
            sut.TryGetConfigValue("AKey7", out configValue);
            Assert.That(configValue.Equals("AValue7",StringComparison.InvariantCulture));
        }


        [Test]
        public void GetValue_ReturnsNullIfNotFound()
        {
            ConfigContext sut = new ConfigContext("LevelKey", "LevelValue");
            Assert.IsNull(sut.GetConfigValue("MissingKey"));
        }

        [Test]
        public void GetValue_ReturnsValueIfFound()
        {
            ConfigContext sut = new ConfigContext("LevelKey", "LevelValue");
            sut.UpsertConfigValue("AKey","AValue");
            Assert.That(sut.GetConfigValue("AKey").Equals("AValue",StringComparison.InvariantCulture));
        }
        [Test]
        public void GetValue_ReturnsCorrectValueWhenMultiopleEntries()
        {
            ConfigContext sut = new ConfigContext("LevelKey", "LevelValue");
            sut.UpsertConfigValue("AKey1", "AValue1");
            sut.UpsertConfigValue("AKey2", "AValue2");
            sut.UpsertConfigValue("AKey3", "AValue3");
            sut.UpsertConfigValue("AKey4", "AValue4");
            sut.UpsertConfigValue("AKey5", "AValue5");
            sut.UpsertConfigValue("AKey6", "AValue6");
            sut.UpsertConfigValue("AKey7", "AValue7");
            sut.UpsertConfigValue("AKey8", "AValue8");
            sut.UpsertConfigValue("AKey9", "AValue9");
            sut.UpsertConfigValue("AKey10", "AValue10");
            sut.UpsertConfigValue("AKey11", "AValue11");
            Assert.That(sut.GetConfigValue("AKey6").Equals("AValue6", StringComparison.InvariantCulture));
        }

    }
}
