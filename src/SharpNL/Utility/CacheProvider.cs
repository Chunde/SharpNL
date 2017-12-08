// 
//  Copyright 2015 Gustavo J Knuppe (https://github.com/knuppe)
// 
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//  
//       http://www.apache.org/licenses/LICENSE-2.0
//  
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// 
//   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
//   - May you do good and not evil.                                         -
//   - May you find forgiveness for yourself and forgive others.             -
//   - May you share freely, never taking more than you give.                -
//   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
//  

using System;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;

namespace SharpNL.Utility {
    /// <summary>
    /// Provides caching support for derived classes.
    /// </summary>
    public abstract class CacheProvider<T> : Disposable {

        #region + Constructors .
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheProvider{T}"/> class with the cache enabled.
        /// </summary>
        protected CacheProvider() : this(true) {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheProvider{T}"/> class specifying if the cache is enabled.
        /// </summary>
        /// <param name="useCache">if set to <c>true</c> the cache will be enabled.</param>
        protected CacheProvider(bool useCache) {
            if (useCache)                
                Cache = new MemoryCache(new MemoryCacheOptions());
        }

        #endregion

        #region . Cache .

        /// <summary>
        /// Gets the memory cache.
        /// </summary>
        /// <value>The memory cache.</value>
        protected MemoryCache Cache { get; private set; }

        #endregion

        #region . DisposeManagedResources .
        /// <summary>
        /// Releases the managed resources.
        /// </summary>
        protected override void DisposeManagedResources() {
            if (Cache != null)
                Cache.Dispose();

            base.DisposeManagedResources();
        }
        #endregion

        #region . Get .
        /// <summary>
        /// Gets a value from the cache.
        /// </summary>
        /// <param name="cacheKey">A unique identifier for the cache entry to add or get.</param>
        /// <returns>
        /// If a matching cache entry already exists, a cache entry; otherwise, the return value from <see cref="M:GetValue"/> will cached and returned.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="cacheKey"/> is null.</exception>
        protected T Get(string cacheKey) {
            if (Cache == null)
                return GetValue(cacheKey);
            
            // Knuppe: I know, this is very clever :P
            var value = new Lazy<T>(() => GetValue(cacheKey), LazyThreadSafetyMode.ExecutionAndPublication);            
            var cached = this.Cache.GetOrCreate(cacheKey, entry => value) as Lazy<T>;
            return (cached ?? value).Value;
        }
        #endregion

        #region . GetValue .
        /// <summary>
        /// Gets a value to be cached.
        /// </summary>
        /// <param name="cacheKey">A unique identifier for the cache entry to add or get.</param>
        /// <returns>The created value to be stored in the cache.</returns>
        protected abstract T GetValue(string cacheKey);
        #endregion

        #region . IsCached .
        /// <summary>
        /// Determines whether a cache entry exists with the given cache key.
        /// </summary>
        /// <param name="cacheKey">A unique identifier for the cache entry to search for.</param>
        /// <returns><c>true</c> if the cache contains a cache entry whose key matches key; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">The <paramref name="cacheKey"/> is null.</exception>
        protected bool IsCached(string cacheKey) {
            object outval = null;

            return Cache != null && Cache.TryGetValue(cacheKey, out outval);
        }
        #endregion

    }
}
