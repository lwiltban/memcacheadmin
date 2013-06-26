using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using MemcacheAdmin.Types;
using Memcached.ClientLibrary;
using Wise.ServiceRepository;

namespace Wise.Web
{
    public class WebCache : ICache
    {
        private CacheType CachingType { get; set; }

        public WebCache()
        {
            CachingType = CacheType.WebCache;
            var memcache = ConfigurationManager.AppSettings["IsMemcache"];
            if (memcache != null && memcache.ToLower().CompareTo("true") == 0)
            {
                CachingType = CacheType.MemCacheD;

                var memcacheServers = ConfigurationManager.AppSettings["MemcacheServers"];
                String[] serverlist = null;

                if (!string.IsNullOrEmpty(memcacheServers))
                {
                    var servers = memcacheServers.Split(',');
                    serverlist = servers;
                }

                SockIOPool pool = SockIOPool.GetInstance();
                pool.SocketConnectTimeout = 500;
                pool.MinConnections = 10;
                pool.InitConnections = 10;
                pool.SetServers(serverlist);
                pool.Initialize();

                Client = new MemcachedClient();
            }
            else
            {
                Cache = HttpContext.Current.Cache;
            }
            
        }

       public Cache Cache { get; set; }
       public MemcachedClient Client { get; set; }

        #region ICache Members

	   public void AddObject<T>( string key, T o )
	   {
           if (CachingType == CacheType.MemCacheD)
           {
               AddObject(key, DefaultCacheTimeout(), o);
           }
           else
           {
               Cache.Insert( key, o, null, Cache.NoAbsoluteExpiration, new TimeSpan( 0, 5, 0 ), CacheItemPriority.Default, null );
           }
	   }

	   public T AddObject<T>( string key, int minutes, T o )
	   {
           if (CachingType == CacheType.MemCacheD)
           {
               Client.Set(key, o, DateTime.Now.AddMinutes(minutes));
           }
           else
           {
               Cache.Insert( key, o, null, Cache.NoAbsoluteExpiration, new TimeSpan( 0, minutes, 0 ), CacheItemPriority.Default, null );
           }
           return o;
	   }

        public T GetObject<T>(string key)
        {
            if (CachingType == CacheType.MemCacheD)
            {
                return (T)Client.Get(key);
            }
            else
            {
                return (T)Cache.Get(key);
            }
        }

        public object GetObject(string key)
        {
            if (CachingType == CacheType.MemCacheD)
            {
                return Client.Get(key);
            }
            else
            {
                return Cache.Get(key);
            }
        }

	   public bool Contains( string key )
	   {
           if (CachingType == CacheType.MemCacheD)
           {
               return Client.KeyExists(key);
           }
           else
           {
               return Cache.Get( key ) != null;

           }
	   }

        public T Expire<T>(string key)
        {
            T o = this.GetObject<T>(key);
            if (CachingType == CacheType.MemCacheD)
            {
                Client.Delete(key);
            }
            else
            {
                Cache.Remove(key);
            }
            return o;
        }

        public T Expire<T>(string key, Func<T, bool> expireIf)
        {
            T o = this.GetObject<T>(key);
            if (expireIf(o))
                if (CachingType == CacheType.MemCacheD)
                {
                    Client.Delete(key);
                }
                else
                {
                    Cache.Remove(key);
                }
            return o;
        }

        public IEnumerator<string> GetKeys()
        {
            var enumerator = Cache.GetEnumerator();
            while (enumerator.MoveNext())
                yield return enumerator.Key.ToString();
        }

        public void Close()
        {
            if (CachingType == CacheType.MemCacheD)
            {
                SockIOPool pool = SockIOPool.GetInstance();
                pool.Shutdown();
            }

        }

        public CacheType GetCacheType()
        {
            return CachingType;
        }

        #endregion

        public bool SetLock(string key, long count)
        {
            return Client.StoreCounter(key, count);
        }

        public long GetLock(string key)
        {
            return Client.GetCounter(key);
        }

        public long LockKey(string lockkey)
        {
            //TODO: Lee may want to look at this to make it a bit more robust.  This is close but probably not all the way there.
            
            if (!Client.KeyExists(lockkey))
            {
                SetLock(lockkey, 1);
                return 1;
            }
            else
            {
                return Client.Increment(lockkey);
            }

        }

        public long UnlockKey(string lockkey)
        {
            //TODO: Lee may want to look at this to make it a bit more robust.  This is close but probably not all the way there.
            if (!Client.KeyExists(lockkey))
            {
                SetLock(lockkey, 0);
                return 0;
            }
            else
            {
                long val = Client.Decrement(lockkey);
                if (val < 0)
                {
                    Client.Set(lockkey, 0);
                    val = 0;
                }
                return val;
            }
            
        }

        public bool IsKeyLocked(string lockkey)
        {
            if (!Contains(lockkey))
                return false;//it isn't locked if it doesn't exist in the cache yet
            var count = Client.GetCounter(lockkey);
            return count > 0;
        }

        public int DefaultCacheTimeout()
        {
            return Int32.Parse(ConfigurationManager.AppSettings["DefaultCacheTimeout"]);
        }

    }
}
