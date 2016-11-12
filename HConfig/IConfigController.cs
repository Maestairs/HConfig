using System.Collections.Generic;

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

        Dictionary<string,string> Context { get; set; } // The current context used to in each plane 

        
        Queue<string> Priority { get; set; }           // The order in which planes are to be searched


        /// Todo Report Macro State

        ConfigKeyReport GetConfigKeyReport(string key);     // Reports on where a specific config was found

    }
}
