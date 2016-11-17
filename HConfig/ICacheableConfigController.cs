namespace HConfig
{
    public interface ICacheableConfigController:IConfigController
    {
        IConfigCache Cache { get; set; }
    }
}
