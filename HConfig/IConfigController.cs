using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HConfig
{

    // The IConfigController Is the public interface for the Hierarchical config
    // To use simply add the config values , then set the Priorities and Context 
    // Then call GetConfigValue or TryGetConfigValue as needed
    
    public interface IConfigController
    {
        #region reads
        string GetConfigValue(string key);
        bool TryGetConfigValue(string key, out string value);
        #endregion

        #region writes
        void UpsertConfigValue(string planeName, string spokeName, string key, string value);
        void UpsertDefaultConfigValue(string planeName, string key, string value);
        #endregion

        Dictionary<string,string> Context { get; set; } 

        List<string> Priorities { get; set; }  //The order in which planes are to be searched
        /// Todo Report Macro State
        /// Todo Report StateOfEach Key
     
    }
}
