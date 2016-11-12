using System.Collections.Generic;

namespace HConfig
{
    // A config plane contains all the config spokes . It can hold its own values Which are the default values for the plane

    internal interface IConfigPlane 
    {
        KeyValuePair<string, string> PlaneDescriptor { get; set; }  //Although it uses a PlaneDescriptor , the value is an empty string 
        #region Spoke Manipulation 
        void UpsertSpoke(IConfigSpoke configSpoke);
        IConfigSpoke GetSpoke(string spokeName);
        bool TryGetSpoke(string spokeName, out IConfigSpoke spoke);
        #endregion

        string Context { get; set; } //This is the spoke name that will be used for this plane unless explicitly overridden

        #region Save Config Items 
        void UpsertConfigValue(string key, string value);
        void UpsertDefaultConfigValue(string key, string value);    // Store a default Value 
        void UpsertConfigValue(string context , string key, string value);
        
        #endregion
        #region Read Config Items

        bool TryGetConfigValue(string key, out string value);
        string GetConfigValue(string key);

       
        bool TryGetConfigValue(string context, string key, out string value);
        string GetConfigValue(string context, string key);

        #endregion

        #region Reporting

        ConfigKeyReport GetConfigKeyReport(string key);

        #endregion


    }
}
