using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HConfig
{
    interface ICacheableConfigController:IConfigController
    {
        IConfigCache Cache { get; set; }
    
    }
}
