using System;
using HConfig;
using NUnit.Framework;

namespace HConfigTests
{
    [TestFixture]
    public class ConfigCacheTests
    {
        [Test]
        public void CacheInsert_AddsValueToCache()
        {
            ConfigCache sut = new ConfigCache();
            sut.CacheInsert("SomeKey","SomeValue");

            Assert.That(sut.GetCachedValue("SomeKey").Equals("SomeValue",StringComparison.InvariantCulture));
        }
        [Test]
        public void CacheInsert_AddingSecondValueForSameKeyOverwritesCachedValue()
        {
            ConfigCache sut = new ConfigCache();
            sut.CacheInsert("SomeKey", "SomeValue");
            sut.CacheInsert("SomeKey", "SomeNewValue");

            Assert.That(sut.GetCachedValue("SomeKey").Equals("SomeNewValue", StringComparison.InvariantCulture));
        }
        [Test]
        public void ClearCache_ClearsTheCache()
        {
            ConfigCacheTestHelper sut = new ConfigCacheTestHelper();
            sut.CacheInsert("SomeKey", "SomeValue");
            sut.CacheInsert("SomeKey", "SomeNewValue");
            sut.CacheInsert("SomeKey2", "SomeNewValue2");
            sut.CacheInsert("SomeKey3", "SomeNewValue3");

            sut.ClearCache();

            Assert.That(sut.GetCacheLength() == 0);
        }
        [Test]
        public void GetCachedValue_ReturnsNullIfNotFound()
        {
            ConfigCacheTestHelper sut = new ConfigCacheTestHelper();
            sut.CacheInsert("SomeKey", "SomeValue");
            sut.CacheInsert("SomeKey", "SomeNewValue");
            sut.CacheInsert("SomeKey2", "SomeNewValue2");
            sut.CacheInsert("SomeKey3", "SomeNewValue3");

            Assert.IsNull(sut.GetCachedValue("SomeUnknownKey"));
        }
        [Test]
        public void TryGetCachedValue_ReturnsTrueIfFound()
        {
            ConfigCacheTestHelper sut = new ConfigCacheTestHelper();
            sut.CacheInsert("SomeKey", "SomeValue");
            sut.CacheInsert("SomeKey", "SomeNewValue");
            sut.CacheInsert("SomeKey2", "SomeNewValue2");
            sut.CacheInsert("SomeKey3", "SomeNewValue3");

    
            string value;
            Assert.IsTrue(sut.TryGetCachedValue("SomeKey", out value));
        }
        [Test]
        public void TryGetCachedValue_SetsValueIfFound()
        {
            ConfigCacheTestHelper sut = new ConfigCacheTestHelper();
            sut.CacheInsert("SomeKey", "SomeValue");
            sut.CacheInsert("SomeKey", "SomeNewValue");
            sut.CacheInsert("SomeKey2", "SomeNewValue2");
            sut.CacheInsert("SomeKey3", "SomeNewValue3");

           
            string value;
            sut.TryGetCachedValue("SomeKey", out value);
            Assert.IsTrue(value == "SomeNewValue");
        }
        [Test]
        public void TryGetCachedValue_ReturnsFalseINotfFound()
        {
            ConfigCacheTestHelper sut = new ConfigCacheTestHelper();
            sut.CacheInsert("SomeKey", "SomeValue");
            sut.CacheInsert("SomeKey", "SomeNewValue");
            sut.CacheInsert("SomeKey2", "SomeNewValue2");
            sut.CacheInsert("SomeKey3", "SomeNewValue3");


            string value;
            
            Assert.IsFalse(sut.TryGetCachedValue("SomeUnkownKey", out value));
        }
        [Test]
        public void TryGetCachedValue_SetsValueToNullIfNotFound()
        {
            ConfigCacheTestHelper sut = new ConfigCacheTestHelper();
            sut.CacheInsert("SomeKey", "SomeValue");
            sut.CacheInsert("SomeKey", "SomeNewValue");
            sut.CacheInsert("SomeKey2", "SomeNewValue2");
            sut.CacheInsert("SomeKey3", "SomeNewValue3");

            string value;
            sut.TryGetCachedValue("SomeUnkownKey", out value);
            Assert.IsNull(value);
        }


    }

    public class ConfigCacheTestHelper : ConfigCache
    {
        public int GetCacheLength()
        {
            return _cache.Count;
        }
    }
}
