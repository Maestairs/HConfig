using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HConfig
{
    internal interface IControlledConfigPlane
    {
        IControlledConfigPlane Child { get; set; }
        IConfigPlane Plane { get;  }

        bool TryGetConfigValue(string key, out string value);
        string GetConfigValue(string key);
    }
}
