using System.Collections.Generic;

namespace HConfig
{
    /// <summary>
    /// Reads config data from a source and stores it in an IConfigController .
    /// 
    /// StoreAllConfig will read all known config into the controller 
    /// StoreConfigForRestrictedContext will only store those config values that match a specific context .
    /// 
    /// Which to use depends on circumstance . If context is known and fixed at application start then useing StoreConfigForRestrictedContext
    /// will  improve initial config load times . It also supports the case where one context value might have very large number of 
    /// possibilities ( e.g thousands of users each with a specific context) Since we only need to load config for the one user .
    /// If the context is not known at start time then it is a balancing act as to which is best . In most cases StoreAllConfig is probably better as
    /// there is no need to refer back to the config store each time the context changes we can let the ConfigController deal with that.
    /// However if we have a large number of possible values for a config item then we will be storing all of those in memory as dictionaries .
    /// This may be an issue for some applications, firstly in terms of memory , and secondly in initial load times with large number 
    /// of config values being loaded that we may never use .
    /// 
    /// </summary>
    public interface IConfigSource
    {
        void StoreAllConfig(IConfigController configController);            //Stores all config , including ones we do not expect to be used
        void StoreConfigForRestrictedContext(IConfigController configController, Dictionary<string, string> localContext);   // Stores only that config that we expect to use

    }
}