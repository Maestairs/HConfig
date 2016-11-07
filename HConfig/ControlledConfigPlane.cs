using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HConfig
{
    internal class ControlledConfigPlane : IControlledConfigPlane
    {
        public  ControlledConfigPlane(string name)
        {
            Plane = new ConfigPlane(name);
        }
        public IControlledConfigPlane Child { get; set; }

        public IConfigPlane Plane { get; }

        public string GetConfigValue(string key)
        {
            string configValue = Plane.GetConfigValue(key);
            return configValue == null && Child != null ? Child.GetConfigValue(key) : null;
        }

        public bool TryGetConfigValue(string key, out string value)
        {
            if (Plane.TryGetConfigValue(key, out value))
                return true;
            return Child != null && Child.TryGetConfigValue(key, out value);
        }
    }
}
