using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HConfig
{
    public class CacheableConfigController : ConfigController, ICacheableConfigController
    {
        IConfigCache _cache;

        public CacheableConfigController()
        {

        }

        public CacheableConfigController(IConfigCache cache)
        {
            Cache = cache;
        }

        public IConfigCache Cache
        {
            get { return _cache; }

            set
            {
                _cache = value;  
                //_cache.ClearCache(); Cant decide if I should clear on each set ?
            }
        }

        public override string GetConfigValue(string key)
        {
            string value;
            if (_cache != null)
            {
                value = _cache.GetCachedValue(key);
                if (value != null)
                    return value;
            }
            value = base.GetConfigValue(key);
            if (value != null)
            {
                _cache?.CacheInsert(key,value);
            }
            return value;
        }

        public override bool TryGetConfigValue(string key, out string value)
        {
             
            if(_cache!= null)
                if (_cache.TryGetCachedValue(key, out value)) return true;
            if (!base.TryGetConfigValue(key, out value)) return false;
            _cache?.CacheInsert(key, value);
            return true;
        }

        public override void SetContext(Dictionary<string, string> context)
        {
            base.SetContext(context);
            _cache?.ClearCache();
        }

        protected override void PrioritisePlanes()
        {
            base.PrioritisePlanes();
            _cache?.ClearCache();
        }
      
    }
}
