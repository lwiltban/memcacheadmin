using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MemcacheAdmin.Utils;

namespace MemcacheAdmin.Models
{
    public class CacheItem
    {
        public object Parse(string key, string line)
        {
            Hashtable hm = new Hashtable();
            MemcacheUtils.LoadItems(line, hm, true);
            return hm[key];
        }
    }
}