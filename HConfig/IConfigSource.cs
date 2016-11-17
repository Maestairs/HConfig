using System.Collections.Generic;

namespace HConfig
{

    public interface IConfigSource
    {
        void StoreAllConfig(IConfigController configController);            //Stores all config , inclusing ones we do not expect to be used
        void StoreConfigForLocalContext(IConfigController configController, Dictionary<string, string> localContext);   // Stores only that config that we expect to use

    }
}