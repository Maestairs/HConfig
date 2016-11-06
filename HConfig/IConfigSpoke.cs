﻿using System.Collections.Generic;

namespace HConfig
{
    public interface IConfigSpoke  
    {
        KeyValuePair<string,string>PlaneDescriptor { get; set; }     

        void UpsertConfigValue(string key, string value);

        bool TryGetConfigValue(string key, out string value);
        string GetConfigValue(string key);
    }
}
