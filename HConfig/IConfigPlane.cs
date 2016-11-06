using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HConfig
{
    // A config plane contains all the config spokes . It also contains the Hub
    // The hub is where default values for that plane are stored
    // The anticipated behaviour on getting a value is that The Plane will first search a specific spoke , 
    // if none found it will then search the hub

    interface IConfigPlane:IConfigSpoke
    {
        void UpsertConfigSpoke(IConfigSpoke configSpoke);
        IConfigSpoke GetConfigSpoke(string spokeName);
        bool TryGetConfigSpoke(string spokeName, out IConfigSpoke spoke);
    }
}
