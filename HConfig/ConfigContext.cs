using System;
using System.Collections.Generic;
using System.Linq;

namespace HConfig
{
    internal class ConfigContext : IConfigContext
    {
        private Dictionary<string, string> _configValues;
        private KeyValuePair<string, string> _levelDescriptor;

        public ConfigContext(string key , string value)
        {
            Init(key , value);
        }

        private void Init(string key , string value)
        {
            _configValues = new Dictionary<string, string>();
            _levelDescriptor = new KeyValuePair<string, string>(key,value);

        }

        public KeyValuePair<string, string> PlaneDescriptor
        {
            get
            {
                return _levelDescriptor;
            }

            set  
            {
                if(string.IsNullOrEmpty(value.Key) )
                    throw new ArgumentException("Invalid LevelDescriptor Key");
                if (string.IsNullOrEmpty(value.Value))
                    throw new ArgumentException("Invalid LevelDescriptor Value");
                if(_levelDescriptor.Key.Length >0)  
                    throw new InvalidProgramException("LevelDescriptor Changed After initialisation");
                _levelDescriptor = value;
            }
        }

        public bool TryGetConfigValue(string key, out string value)
        {
            return (_configValues.TryGetValue(key, out value));
        }

        public void UpsertConfigValue(string key , string value)
        {
            _configValues.Remove(key);
            _configValues.Add(key,value);
        }

        public string GetConfigValue(string key)
        {
            return _configValues.FirstOrDefault(k => k.Key.Equals(key, StringComparison.InvariantCulture)).Value;
        }
    }
}
