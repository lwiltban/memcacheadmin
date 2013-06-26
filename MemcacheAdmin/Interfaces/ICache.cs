using System;
using System.Collections;
using System.Collections.Generic;
using MemcacheAdmin.Types;

namespace Wise.ServiceRepository
{
    /// <summary>
    /// Interface that specifies the needed methods to cache
    /// any type of object
    /// </summary>
    /// <remarks>
    /// This should be instantiated as a singleton PER application
    /// </remarks>
    public interface ICache
    {
	    /// <summary>
	    /// Method to add an object to the cache
	    /// given the particular key. This is a
	    /// destructive call in the sense that if
	    /// a particular key already exists, it will
	    /// be overwritten by a new  object.
        /// If the cache is distributed, the object added
        /// will have a lifetime of DefaultCacheTimeout minutes
	    /// </summary>
	    /// <typeparam name="T">Type of object to be stored</typeparam>
	    /// <param name="key">Identifier to retrieve object</param>
	    /// <param name="o">Object to be cached</param>
	    void AddObject<T>( string key, T o );

	    /// <summary>
	    /// Method to add an object to the cache
	    /// given the particular key. This is a
	    /// destructive call in the sense that if
	    /// a particular key already exists, it will
	    /// be overwritten by a new  object.
	    /// </summary>
	    /// <typeparam name="T">Type of object to be stored</typeparam>
	    /// <param name="key">Identifier to retrieve object</param>
	    /// <param name="key">Time in minutes to store object</param>
	    /// <param name="o">Object to be cached</param>
	    T AddObject<T>( string key, int minutes, T o );

	    /// <summary>
        /// This method reteieves an object by
        /// the given key.
        /// </summary>
        /// <typeparam name="T">Type of object to be returned</typeparam>
        /// <param name="key">Identifier to retrieve object</param>
        /// <returns>Object</returns>
        T GetObject<T>(string key);

        object GetObject(string key);
        /// <summary>
        /// This method checks the existance of
        /// a particular keyed item in the cache
        /// </summary>
        /// <param name="key">Identifier to retrieve object</param>
        /// <returns>Yes/No</returns>
        bool Contains(string key);

        /// <summary>
        /// Expunge an object from the cache. This will return the object
        /// but remove it from the cache.
        /// </summary>
        /// <typeparam name="T">Type of object to expire</typeparam>
        /// <param name="key">Identifier of object</param>
        /// <returns>Object</returns>
        T Expire<T>(string key);

        /// <summary>
        /// Expunge an object from the cache and send messages to 
        /// other servers. This will return the object
        /// but remove it from the cache.
        /// </summary>
        /// <typeparam name="T">Type of object to expire</typeparam>
        /// <param name="key">Identifier of object</param>
        /// <returns>Object</returns>

        /// <summary>
        /// Expunge an object from the cache. This will return the object
        /// but remove it from the cache.
        /// </summary>
        /// <typeparam name="T">Type of object to expire</typeparam>
        /// <param name="key">Identifier of object</param>
        /// <param name="expireIf">
        /// Method that defines whether or not a particular cache
        /// item should be expunged.
        /// </param>
        /// <returns>Object</returns>
        T Expire<T>(string key, Func<T, bool> expireIf);

        /// <summary>
        /// This method returns the set of keys
        /// that are currently being used in the
        /// cache
        /// </summary>
        /// <returns>Set ok valid keys</returns>
        IEnumerator<string> GetKeys();

        /// <summary>
        /// This method shuts down
        /// the cache
        /// </summary>
        /// <returns></returns>
        void Close();

        /// <summary>
        /// This method returns the type of caching using
        /// by the cache
        /// </summary>
        /// <returns>Enum of cachingtype</returns>
        CacheType GetCacheType();

        bool SetLock(String key, long count);
        long LockKey(String key);
        long UnlockKey(String key);
        bool IsKeyLocked(String key);

        long GetLock(string key);
        int DefaultCacheTimeout();

    }
}
