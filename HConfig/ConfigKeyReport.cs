namespace HConfig
{

    public enum ConfigSource {  ContextSpecific , Default}
    // This is intended to be used when looking at effective configuration 
    public class ConfigKeyReport
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string ConfigContextName { get; set; }
        public string PlaneName { get; set; }
        public ConfigSource ConfigSource { get; set; }  

    }
}
