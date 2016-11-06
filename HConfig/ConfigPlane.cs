using System;
using System.Collections.Generic;
using System.Linq;

namespace HConfig
{
    public class ConfigPlane :  IConfigPlane
    {
        private KeyValuePair<string, string> _planeDescriptor;
        private Dictionary<string, IConfigSpoke> _spokes;
        private Dictionary<string, string> _defaultValues;
        private string _context;

        public ConfigPlane(string name)
        {
            Init(name);
        }

        private void Init(string name)
        {
            _planeDescriptor = new KeyValuePair<string, string>(name,"");
            _spokes= new Dictionary<string, IConfigSpoke>();
            _defaultValues = new Dictionary<string, string>();
            _context=string.Empty;
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

        public string Context
        {
            get
            {
                return _context;
            }
            set
            {
                VerifyContext(value);
                _context = value;
            }
        }

        public void UpsertSpoke(IConfigSpoke configSpoke)
        {
            if(configSpoke.PlaneDescriptor.Key != PlaneDescriptor.Key) //Incorrect plane 
                throw new ArgumentException(
                    $"Incorrect Plane Descriptor : Expected {PlaneDescriptor.Key} Received {configSpoke.PlaneDescriptor.Key}");

            _spokes.Remove(configSpoke.PlaneDescriptor.Value);
            _spokes.Add(configSpoke.PlaneDescriptor.Value,configSpoke);
        }

        public IConfigSpoke GetSpoke(string spokeName)
        {
            return _spokes.FirstOrDefault(s => s.Key.Equals(spokeName, StringComparison.InvariantCulture)).Value;
        }


        public bool TryGetSpoke(string spokeName, out IConfigSpoke spoke)
        {
            return (_spokes.TryGetValue(spokeName, out spoke));
        }

        
      

        public void UpsertConfigValue(string spokeName, string key, string value)
        {
            IConfigSpoke spoke;
            VerifyContext(spokeName);
            if (TryGetSpoke(spokeName, out spoke))
            {
                spoke.UpsertConfigValue(key,value);
            }
            else
            {
                SaveToNewSpoke(spokeName, key, value);
            }
        }
        public void UpsertConfigValue(string key, string value)
        {
             UpsertConfigValue(Context,key,value);
        }
        private void SaveToNewSpoke(string spokeName, string key, string value)
        {
            IConfigSpoke spoke = new ConfigSpoke(PlaneDescriptor.Key, spokeName);
            spoke.UpsertConfigValue(key, value);
            _spokes.Add(spokeName, spoke);
        }

        public bool TryGetConfigValue(string spokeName, string key, out string value)
        {
            string configValue;
            IConfigSpoke spoke;
            VerifyContext(spokeName);
            //Search Spoke , then default values
            if (TryGetSpoke(spokeName, out spoke) && spoke.TryGetConfigValue(key,out configValue))
            {
                value = configValue;
                return true;
            }
            return (_defaultValues.TryGetValue(key, out value));
        }

        public bool TryGetConfigValue(string key, out string value)
        {
            return TryGetConfigValue(Context, key, out value);
        }

        public string GetConfigValue(string spokeName, string key)
        {
            VerifyContext(spokeName);
            IConfigSpoke spoke;
            if (TryGetSpoke(spokeName, out spoke)  )
            {
                var configValue = spoke.GetConfigValue(key);
                if (configValue != null)
                    return configValue;
            }

            return _defaultValues.FirstOrDefault(d => d.Key.Equals(key, StringComparison.InvariantCulture)).Value;
        }

        public string GetConfigValue(string key)
        {
            return GetConfigValue(Context, key);
        }


        // ReSharper disable once UnusedParameter.Local
        private void VerifyContext(string context)
        {
            if (context == null || context.Equals(string.Empty, StringComparison.InvariantCulture))
                throw new ArgumentException("Invalid Spoke Name Context: null or empty");
        }

        public void UpsertDefaultConfigValue(string key, string value)
        {
            _defaultValues.Remove(key);
            _defaultValues.Add(key,value);
        }
    }
}
