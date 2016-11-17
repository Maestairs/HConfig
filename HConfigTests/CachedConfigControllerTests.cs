using System;
using System.Collections.Generic;
using HConfig;
using NUnit.Framework;
using Rhino.Mocks;

namespace HConfigTests
{
    [TestFixture]
    public class CachedConfigControllerTests
    {
        [Test]
        public void Construction_WithNoCacheDoesNotThrowException()
        {

            Assert.DoesNotThrow(()=> new CacheableConfigController());
        }
        [Test]
        public void Construction_WithACacheDoesNotThrowException()
        {
            Assert.DoesNotThrow(() => new CacheableConfigController(new ConfigCache()));
        }
        [Test]
        public void IfNoCacheSpecified_CanWriteAndReadConfigValue()
        {
            CacheableConfigController sut = new CacheableConfigController();


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
        public void IfCacheSpecifiedInConstructor_CanWriteAndReadConfigValue()
        {
            CacheableConfigController sut = new CacheableConfigController(new ConfigCache());


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
        public void IfCacheSpecifiedInProperty_CanWriteAndReadConfigValue()
        {
            CacheableConfigController sut = new CacheableConfigController();

            sut.Cache = new ConfigCache();
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
        public void IfCacheSpecified_AReadResultsInKeyValueWrittenToCache()
        {
            IConfigCache configCache = MockRepository.GenerateStub<IConfigCache>();
            CacheableConfigController sut = new CacheableConfigController();
            sut.Cache = configCache;

            Queue<string> priority = new Queue<string>();
            priority.Enqueue("FirstPlane");


            sut.Priority = priority;

            sut.UpsertDefaultConfigValue("FirstPlane", "ConfigKey", "ConfigValue");
            Dictionary<string, string> context = new Dictionary<string, string>();
            context.Add("FirstPlane", "MyConfigContext");
            sut.SearchContext = context;

            string result = sut.GetConfigValue("ConfigKey");

            configCache.AssertWasCalled(x => x.CacheInsert(Arg<string>.Is.Equal("ConfigKey"), Arg<string>.Is.Equal("ConfigValue")));
        }
        [Test]
        public void IfCacheSpecified_WillUseCachedValueIfPresent()
        {
            IConfigCache configCache = MockRepository.GenerateStub<IConfigCache>();
            configCache.Stub(x => x.GetCachedValue("MyCachedConfigKey")).Return("MyCachedConfigValue");
            CacheableConfigController sut = new CacheableConfigController();
            sut.Cache = configCache;

            Queue<string> priority = new Queue<string>();
            priority.Enqueue("FirstPlane");


            sut.Priority = priority;

            sut.UpsertDefaultConfigValue("FirstPlane", "ConfigKey", "ConfigValue");
            Dictionary<string, string> context = new Dictionary<string, string>();
            context.Add("FirstPlane", "MyConfigContext");
            sut.SearchContext = context;

            sut.GetConfigValue("ConfigKey");
            string result = sut.GetConfigValue("MyCachedConfigKey");

            Assert.That(result.Equals("MyCachedConfigValue",StringComparison.InvariantCulture));
        }
        [Test]
        public void IfCacheSpecified_ChangingPriorityClearsCache()
        {
            IConfigCache configCache = MockRepository.GenerateStub<IConfigCache>();
            CacheableConfigController sut = new CacheableConfigController();
            sut.Cache = configCache;

            Queue<string> priority = new Queue<string>();
            priority.Enqueue("FirstPlane");

            configCache.AssertWasNotCalled(x=>x.ClearCache());

            sut.Priority = priority;

            configCache.AssertWasCalled(x=>x.ClearCache());
        }
        [Test]
        public void IfCacheSpecified_ChangingContextClearsCache()
        {
            IConfigCache configCache = MockRepository.GenerateStub<IConfigCache>();
            CacheableConfigController sut = new CacheableConfigController();
            sut.Cache = configCache;

            Queue<string> priority = new Queue<string>();
            priority.Enqueue("FirstPlane");

            configCache.AssertWasNotCalled(x => x.ClearCache());

            sut.SetContext( new Dictionary<string, string>());

            configCache.AssertWasCalled(x => x.ClearCache());
        }
        [Test]
        public void IfCacheSpecified_UpsertingConfigValueClearsCache()
        {
            IConfigCache configCache = MockRepository.GenerateStub<IConfigCache>();
            CacheableConfigController sut = new CacheableConfigController();
            sut.Cache = configCache;

            Queue<string> priority = new Queue<string>();
            priority.Enqueue("FirstPlane");

            configCache.AssertWasNotCalled(x => x.ClearCache());

            sut.UpsertDefaultConfigValue("FirstPlane", "ConfigKey", "ConfigValue");

            configCache.AssertWasCalled(x => x.ClearCache());
        }
    }
}
