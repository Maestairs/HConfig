namespace HConfig
{

    // A config writers job is to read config from some place and use it to populate a ConfigController
    // It may decide to pass all config to a controller , or only specific config 
    public interface IConfigWriter
    {
        IConfigController ConfigController { get; set; }
    }
}
