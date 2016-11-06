using System.Collections.Generic;

namespace HConfig
{
    // The idea in a hierarchical config is to look for a config value , if none found we go to the next layer down 
    // We continue that until either we have found a value or run out of layers.
    // Each layer represents a type of config . Typical types being User , PC , workgroup etc 
    // In the above example we may wish to have different values for different users
    // A useful way of visualising this is that each layer is a disc comprising a hub and a series of spokes
    // Each spoke can contain values for a specific context e.g user specific configs 
    // IConfigSpoke Defines how that Spoke  

    interface IConfigSpoke
    {
        KeyValuePair<string,string>LevelDescriptor { get; set; }    // The key is the type e.g UserName , The value is ?? the value !e.g JoeBloggs
                                                                    // Should throw exceptions if changed 

        void UpsertConfigValue(string key , string value);
      
        bool TryGetConfigValue(string key, out string value);
        string GetConfigValue(string key);
    }
}
