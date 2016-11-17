﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HConfig
{
    public interface IConfigCache
    {
        void ClearCache();

        string GetCachedValue(string key);
        bool TryGetCachedValue(string key, out string value);

        void CacheInsert(string key, string value);
    }
}
