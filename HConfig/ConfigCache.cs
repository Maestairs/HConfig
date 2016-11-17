using System;
using System.Collections.Generic;
using System.Linq;

namespace HConfig
{
    public class ConfigCache : IConfigCache
    {
        // ReSharper disable once InconsistentNaming
        protected readonly Dictionary<string,string> _cache ;

        public ConfigCache()
        {
            _cache = new Dictionary<string, string>();
        }
        public void CacheInsert(string key, string value)
        {
            _cache.Remove(key);
            _cache.Add(key,value);
        }

        public void ClearCache()
        {
            _cache.Clear();
        }

        public string GetCachedValue(string key)
        {
            return _cache.FirstOrDefault(k => k.Key.Equals(key, StringComparison.InvariantCulture)).Value;
        }

        public bool TryGetCachedValue(string key, out string value)
        {
            return _cache.TryGetValue(key, out value);
        }

    }
}
