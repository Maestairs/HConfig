using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HConfigTests")]  //Allow tests to run against internal code 
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]  //Allow RhinoMocks to mock internal types

namespace HConfig
{
    public class ConfigController : IConfigController
    {
        private readonly Dictionary<string,IControlledConfigPlane> _planes;
        private Queue<string> _priority;

        // ReSharper disable once InconsistentNaming
        internal  IControlledConfigPlane _entryPoint;  //Set to the highest priority plane
        private Dictionary<string, string> _context; 
       
        public  ConfigController()
        {
            _planes = new Dictionary<string, IControlledConfigPlane>();
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

        public Dictionary<string, string> Context
        {
            get { return _context; }

            set
            {
                _context = value;
                SetContext(_context);
            }
        }

        public void SetContext(Dictionary<string, string> context)
        {
            if (context == null) return;
            foreach (var plane in context)
            {
                IControlledConfigPlane configPlane;
                if (_planes.TryGetValue(plane.Key, out configPlane))
                {
                    configPlane.Context = plane.Value;
                }
                // Dont throw exception if no plane found as context may be set 
                // before planes are created
            }
        }

        public void UpsertConfigValue(string planeName,string spokeName, string key, string value)
        {
            IControlledConfigPlane configPlane = GetOrCreateConfigPlane(planeName);
            configPlane.UpsertConfigValue(spokeName,key,value);
                
        }

        public void UpsertDefaultConfigValue(string planeName, string key, string value)
        {
            IControlledConfigPlane configPlane = GetOrCreateConfigPlane(planeName);
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
            throw new NotImplementedException();
        }

        private IControlledConfigPlane GetOrCreateConfigPlane(string planeName)
        {
            IControlledConfigPlane configPlane;
            if (!_planes.TryGetValue(planeName, out configPlane))
            {
                configPlane = new ControlledConfigPlane(planeName);
                _planes.Add(planeName, configPlane);
                PrioritisePlanes();
                SetContext(Context);  // Context may be set before a plane is created , so need to cope with it here
            }
            return configPlane;
        }

        // NB Priority might mention planes that dont exist 
        private void PrioritisePlanes()
        {
            if (Priority != null)
            {
                IControlledConfigPlane previousPlane=null;

                foreach (var plane in _planes) //reset child links
                    plane.Value.Child = null;
               

                foreach (var priority in Priority)
                {
                    IControlledConfigPlane currentPlane;

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

        private bool TryGetPlane(string planeName, out IControlledConfigPlane plane)
        {
            IControlledConfigPlane controlledPlane;
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
            if (Context == null) throw new ApplicationException("Attempt to ReadConfig With No Context Set");
            if (Priority == null) throw new ArgumentException("Attempt to ReadConfig With No Priorities Set");
        }


    }
}
