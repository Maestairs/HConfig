using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HConfigTests")]  //Allow tests to run against internal code 
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]  //Allow RhinoMocks to mock internal types

namespace HConfig
{
    public class ConfigController : IConfigController , IConfigReporter
    {
        private readonly Dictionary<string,IConfigPlane> _planes;
        private Queue<string> _priority;

        // ReSharper disable once InconsistentNaming
        internal  IConfigPlane _entryPoint;  //Set to the highest priority plane
        private Dictionary<string, string> _searchContext;
        private readonly List<string> _knownConfigKeys;          // maintain a list of known keys for reporting purposes 
       



        public  ConfigController()
        {
            _planes = new Dictionary<string, IConfigPlane>();
            _knownConfigKeys = new List<string>();
        }

        public Queue<string> Priority
        {
            get { return (_priority); }

            set
            {
                _priority = value;

                PrioritisePlanes();
            }
        }

        public Dictionary<string, string> SearchContext
        {
            get { return _searchContext; }

            set
            {
                _searchContext = value;
                SetContext(_searchContext);
            }
        }

        public void SetContext(Dictionary<string, string> context)
        {
            if (context == null) return;
            foreach (var plane in context)
            {
                IConfigPlane configPlane;
                if (_planes.TryGetValue(plane.Key, out configPlane))
                {
                    configPlane.SearchContext = plane.Value;
                }
                // Dont throw exception if no plane found as context may be set 
                // before planes are created
            }
        }

        public void UpsertConfigValue(string planeName,string configContextName, string key, string value)
        {
            ValidateKeyRegistraion(key);
            IConfigPlane configPlane = GetOrCreateConfigPlane(planeName);
            configPlane.UpsertConfigValue(configContextName,key,value);
                
        }

        public void UpsertDefaultConfigValue(string planeName, string key, string value)
        {
            ValidateKeyRegistraion(key);
            IConfigPlane configPlane = GetOrCreateConfigPlane(planeName);
            configPlane.UpsertDefaultConfigValue( key, value);
        }

        public bool TryGetConfigValue(string key, out string value)
        {
            ValidateContextAndPriorities();
            return _entryPoint.TryGetConfigValue(key, out value);
        }

        public string GetConfigValue(string key)
        {
            ValidateContextAndPriorities();
            return _entryPoint.GetConfigValue(key);
        }

        public ConfigKeyReport GetConfigKeyReport(string key)
        {
            return _entryPoint.GetConfigKeyReport(key);
        }

        // Uses knownkeys as a list to search for should never fail to find a configkeyreport for each
        public List<ConfigKeyReport> GetKeyReports()
        {
            ConfigKeyReport configKeyReport;
            List<ConfigKeyReport>retVal = new List<ConfigKeyReport>();
            foreach (string key in _knownConfigKeys)
            {
                configKeyReport = GetConfigKeyReport(key);
                if(configKeyReport == null) throw new Exception($"Could not find an expected key For {key}");
                retVal.Add(configKeyReport);
            }
            return retVal;
        }

        // Keep a track of known keys for reporting purposes
        private void ValidateKeyRegistraion(string key)
        {
            if(!_knownConfigKeys.Contains(key))
                _knownConfigKeys.Add(key);
        }

        private IConfigPlane GetOrCreateConfigPlane(string planeName)
        {
            IConfigPlane configPlane;
            if (!_planes.TryGetValue(planeName, out configPlane))
            {
                configPlane = new ConfigPlane(planeName);
                _planes.Add(planeName, configPlane);
                PrioritisePlanes();
                SetContext(SearchContext);  // Context may be set before a plane is created , so need to cope with it here
            }
            return configPlane;
        }

        // NB Priority might mention planes that dont exist 
        private void PrioritisePlanes()
        {
            if (Priority != null)
            {
                IConfigPlane previousPlane=null;

                foreach (var plane in _planes) //reset child links
                    plane.Value.Child = null;
               

                foreach (var priority in Priority)
                {
                    IConfigPlane currentPlane;

                    //Dont throw exception as priorities might be set before config values
                    //And hence the plane may not exist yet . Hence why we redo priority on a new plane
                    if (!TryGetPlane(priority, out currentPlane)) continue;  

                    if (previousPlane == null)
                    {
                        _entryPoint = previousPlane = currentPlane;
                    }
                    else
                    {
                        previousPlane.Child = currentPlane;
                        previousPlane = currentPlane;
                    }
                }
            }
        }

        private bool TryGetPlane(string planeName, out IConfigPlane plane)
        {
            IConfigPlane controlledPlane;
            if (_planes.TryGetValue(planeName, out controlledPlane))
            {
                plane = controlledPlane;
                return true;
            }
            plane = null;
            return false;
        }

        private void ValidateContextAndPriorities()
        {
            if (SearchContext == null) throw new ApplicationException("Attempt to ReadConfig With No Context Set");
            if (Priority == null) throw new ArgumentException("Attempt to ReadConfig With No Priorities Set");
        }

    }
}
