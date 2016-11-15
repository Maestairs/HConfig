using System.Collections.Generic;

namespace HConfig
{
    public interface IConfigReporter
    {
        List<ConfigKeyReport> GetKeysReport();                  // Report on all known config based on the current Context and Priority

        ConfigKeyReport GetConfigKeyReport(string key);         // Reports on where a specific config was found


    }
}
