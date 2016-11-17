using System.Collections.Generic;

namespace HConfig
{

    // The IConfigController Is the public interface for the Hierarchical config
    // To use simply add the config values , then set the Priorities and Context 
    // Then call GetConfigValue or TryGetConfigValue as needed.

    
    
    
    public interface IConfigController
    {
        #region reads
        string GetConfigValue(string key);
        bool TryGetConfigValue(string key, out string value);
        #endregion

        #region writes
        void UpsertConfigValue(string configPlaneName, string configContextName, string key, string value);
        void UpsertDefaultConfigValue(string configPlaneName, string key, string value);
        #endregion

        Dictionary<string,string> SearchContext { get; set; }   // The  context used in each plane 

        Queue<string> Priority { get; set; }                    // The order in which planes are to be searched

     }
}
