namespace HConfig
{
    internal interface IControlledConfigPlane : IConfigPlane
    {
        IControlledConfigPlane Child { get; set; }

    }
}
