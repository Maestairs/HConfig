using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HConfig
{
    // This is intended to be used when looking at effective configuration 
    public class ConfigKeyReport
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string SpokeName { get; set; }
        public string PlaneName { get; set; }
        public bool ValueFoundOnSpoke { get; set; } //If flase then was found in default
        
    }
}
