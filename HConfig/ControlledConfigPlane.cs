namespace HConfig
{

    // A controlled Config plane is a ConfigPlane with an optional Child
    // If it cannot find a value in itself and a child is defined it passes the Get or TryGet to the child

    internal class ControlledConfigPlane : ConfigPlane , IControlledConfigPlane
    {
        public  ControlledConfigPlane(string name):base(name)
        {
        }

        public IControlledConfigPlane Child { get; set; }

       
        public override string GetConfigValue(string key)
        {
            string result= base.GetConfigValue(key);

            return result ?? Child?.GetConfigValue(key);
        }

        public override string GetConfigValue(string spokeName, string key)
        {
            string result = base.GetConfigValue(spokeName,key);

            return result ?? Child?.GetConfigValue(spokeName, key);
        }

        public override bool TryGetConfigValue(string key, out string value)
        {
            bool success = base.TryGetConfigValue(key, out value);
            if (!success)
            {
                return Child != null && ( Child.TryGetConfigValue(key, out value));
            }
            return true;
        }

        public override bool TryGetConfigValue(string spokeName, string key, out string value)
        {
            bool success = base.TryGetConfigValue(spokeName , key, out value);
            if (!success)
            {
                return Child != null && (Child.TryGetConfigValue(spokeName,key, out value));
            }
            return true;
        }


        public override ConfigKeyReport GetConfigKeyReport(string key)
        {
            ConfigKeyReport retVal = base.GetConfigKeyReport(key);

            return retVal != null ? (retVal) : Child?.GetConfigKeyReport(key);
        }
    }
}
