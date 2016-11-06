using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HConfig
{
    class ConfigPlane : IConfigPlane     ///TODO : INherit from IConfigSpoke ?
    {
        public KeyValuePair<string, string> LevelDescriptor
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IConfigSpoke GetConfigSpoke(string spokeName)
        {
            throw new NotImplementedException();
        }

        public string GetConfigValue(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetConfigSpoke(string spokeName, out IConfigSpoke spoke)
        {
            throw new NotImplementedException();
        }

        public bool TryGetConfigValue(string key, out string value)
        {
            throw new NotImplementedException();
        }

        public void UpsertConfigSpoke(IConfigSpoke configSpoke)
        {
            throw new NotImplementedException();
        }

        public void UpsertConfigValue(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
