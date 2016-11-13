using System;
using System.Collections.Generic;
using System.Linq;

namespace HConfig
{
    /// <summary>

    /// So for a given key the ConfigPlane first looks in the configContext for the given context
    /// if none found there it looks in its default values
    /// </summary>
    internal class ConfigPlane :  IConfigPlane
    {
        private KeyValuePair<string, string> _planeDescriptor;
        private Dictionary<string, IConfigContext> _configContexts;
        private Dictionary<string, string> _defaultValues;
        private string _searchContext;

        public ConfigPlane(string name)
        {
            Init(name);
        }

        private void Init(string name)
        {
            _planeDescriptor = new KeyValuePair<string, string>(name,"");
            _configContexts= new Dictionary<string, IConfigContext>();
            _defaultValues = new Dictionary<string, string>();
            _searchContext=string.Empty;
        }

        public KeyValuePair<string, string> PlaneDescriptor
        {
            get
            {
                return _planeDescriptor;
            }

            set
            {
                if (string.IsNullOrEmpty(value.Key))
                    throw new ArgumentException("Invalid LevelDescriptor Key");
                if (_planeDescriptor.Key.Length > 0)
                    throw new InvalidProgramException("LevelDescriptor Changed After initialisation");
                _planeDescriptor = new KeyValuePair<string, string>(value.Key, "");
            }
        }

        public string SearchContext
        {
            get
            {
                return _searchContext;
            }
            set
            {
                VerifyContext(value);
                _searchContext = value;
            }
        }

        public IConfigPlane Child { get; set; }
    

        public string GetConfigValue(string key)
        {
            string result = GetLocalConfigValue(key);

            return result ?? Child?.GetConfigValue(key);
        }
        public  string GetConfigValue(string configContextName, string key)
        {
            string result = GetLocalConfigValue(configContextName, key);

            return result ?? Child?.GetConfigValue(configContextName, key);
        }

        public  bool TryGetConfigValue(string key, out string value)
        {
            bool success = TryGetLocalConfigValue(key, out value);
            if (!success)
            {
                return Child != null && (Child.TryGetConfigValue(key, out value));
            }
            return true;
        }

        public  bool TryGetConfigValue(string configContextName, string key, out string value)
        {
            bool success = TryGetLocalConfigValue(configContextName, key, out value);
            if (!success)
            {
                return Child != null && (Child.TryGetConfigValue(configContextName, key, out value));
            }
            return true;
        }


        public  ConfigKeyReport GetConfigKeyReport(string key)
        {
            ConfigKeyReport retVal = GetLocalConfigKeyReport(key);

            return retVal != null ? (retVal) : Child?.GetConfigKeyReport(key);
        }


        public void UpsertConfigContext(IConfigContext configContext)
        {
            if(configContext.PlaneDescriptor.Key != PlaneDescriptor.Key) //Incorrect plane 
                throw new ArgumentException(
                    $"Incorrect Plane Descriptor : Expected {PlaneDescriptor.Key} Received {configContext.PlaneDescriptor.Key}");

            _configContexts.Remove(configContext.PlaneDescriptor.Value);
            _configContexts.Add(configContext.PlaneDescriptor.Value,configContext);
        }

        public IConfigContext GetConfigContext(string contextConfigName)
        {
            return _configContexts.FirstOrDefault(s => s.Key.Equals(contextConfigName, StringComparison.InvariantCulture)).Value;
        }


        public bool TryGetConfigContext(string searchContextName, out IConfigContext configContext)
        {
            return (_configContexts.TryGetValue(searchContextName, out configContext));
        }

        public void UpsertConfigValue(string configContextName, string key, string value)
        {
            IConfigContext configContext;
            VerifyContext(configContextName);
            if (TryGetConfigContext(configContextName, out configContext))
            {
                configContext.UpsertConfigValue(key,value);
            }
            else
            {
                SaveToNewConfigContext(configContextName, key, value);
            }
        }
        public void UpsertConfigValue(string key, string value)
        {
             UpsertConfigValue(SearchContext,key,value);
        }

        public void UpsertDefaultConfigValue(string key, string value)
        {
            _defaultValues.Remove(key);
            _defaultValues.Add(key, value);
        }

        private  bool TryGetLocalConfigValue(string key, out string value)
        {
            return TryGetLocalConfigValue(SearchContext, key, out value);
        }




        private bool TryGetLocalConfigValue(string searchContextName, string key, out string value)
        {
            string configValue;
            IConfigContext configContext;
            VerifyContext(searchContextName);
            //Search configContext , then default values
            if (TryGetConfigContext(searchContextName, out configContext) && configContext.TryGetConfigValue(key,out configValue))
            {
                value = configValue;
                return true;
            }
            return (_defaultValues.TryGetValue(key, out value));
        }

        private  string GetLocalConfigValue(string searchContextName, string key)
        {
            VerifyContext(searchContextName);
            IConfigContext configContext;
            if (TryGetConfigContext(searchContextName, out configContext)  )
            {
                var configValue = configContext.GetConfigValue(key);
                if (configValue != null)
                    return configValue;
            }

            return _defaultValues.FirstOrDefault(d => d.Key.Equals(key, StringComparison.InvariantCulture)).Value;
        }

        private string GetLocalConfigValue(string key)
        {
            return GetLocalConfigValue(SearchContext, key);
        }
        
  


        private  ConfigKeyReport GetLocalConfigKeyReport(string key)
        {
            string configValue = GetLocalConfigValue(key);
            if (configValue == null) return null;
            ConfigKeyReport retVal = new ConfigKeyReport
            {
                Key = key,
                Value = configValue
            };
            string configContextName;
            retVal.ConfigSource = WasValueFromConfigContext(key, out configContextName)
                ? ConfigKeySource.ContextSpecific
                : ConfigKeySource.Default;
            retVal.PlaneName = PlaneDescriptor.Key;
            retVal.ConfigContextName = configContextName;

            return (retVal);
        }

        // ReSharper disable once UnusedParameter.Local
        private void VerifyContext(string context)
        {
            if (context == null || context.Equals(string.Empty, StringComparison.InvariantCulture))
                throw new ArgumentException("Invalid context Name Context: null or empty");
        }

        private bool WasValueFromConfigContext(string key, out string configContextName)
        {
            IConfigContext configContext;
            configContextName = null;
            if (!TryGetConfigContext(SearchContext, out configContext)) return false;
            configContextName = configContext.PlaneDescriptor.Value;
            return configContext.GetConfigValue(key) != null;
        }

        private void SaveToNewConfigContext(string configContextName, string key, string value)
        {
            IConfigContext configContext = new ConfigContext(PlaneDescriptor.Key, configContextName);
            configContext.UpsertConfigValue(key, value);
            _configContexts.Add(configContextName, configContext);
        }
    }
}
