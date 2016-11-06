using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HConfig
{
    // A config plane contains all the config spokes . It can hold its own values Which are the default values for the plane

    public interface IConfigPlane 
    {
        KeyValuePair<string, string> PlaneDescriptor { get; set; }  //Although it uses a PlaneDescriptor , the value is an empty string (No spoke can have an empty value)

        void UpsertConfigSpoke(IConfigSpoke configSpoke);
        IConfigSpoke GetConfigSpoke(string spokeName);
        bool TryGetConfigSpoke(string spokeName, out IConfigSpoke spoke);

        void SetContext(string spokeName);  //This is the spoke name that will be used for this plane unless explicitly overridden

        void UpsertConfigValue(string key, string value);
        bool TryGetConfigValue(string key, out string value);
        string GetConfigValue(string key);

        void UpsertConfigValue(string spokeName , string key, string value);
        bool TryGetConfigValue(string spokeName , string key, out string value);
        string GetConfigValue(string spokeName , string key);
    }
}
