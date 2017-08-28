// ***********************************************************************
// Assembly         : Exfresh.WMS.Utility
// Author           : WayneChen
// Created          : 05-06-2015
//
// Last Modified By : WayneChen
// Last Modified On : 05-06-2015
// ***********************************************************************
// <copyright file="KVStoreManager.cs" company="Exfresh">
//     Copyright (c) Exfresh. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;

namespace InstagramPhotos.Utility.KVStore
{
    //KVstore引擎
    public static class KVStoreManager
    {
        #region [           Members           ]

        private static object syncObj = new object();
        private static Hashtable ht = new Hashtable();

        static IKvStoreRedisEngine kvstoreEngine = null;

        #endregion

        #region [           Methods           ]

        public static void SetEngine(IKvStoreRedisEngine engine)
        {
            if (engine == null)
                throw new ArgumentNullException("KVStoreRedisEngine cannot be null.");

            //if (kvstoreEngine != null)
            //    throw new ArgumentNullException("KVStoreRedisEngine has already been setted.");

            //lock (syncObj)
            //{
            //    if (kvstoreEngine == null)
            kvstoreEngine = engine;
            // }
        }

        public static KVStoreEntityTable<K, V> GetKVStoreEntityTable<K, V>(string redisKey)
        {
            if (kvstoreEngine == null)
                throw new ArgumentNullException("KVStoreRedisEngine haven't been setted.");

            return GetKVStoreEntityTable<K, V>(redisKey, 0);
        }

        public static KVStoreEntityTable<K, V> GetKVStoreEntityTable<K, V>(string redisKey, int redisDuration)
        {
            if (kvstoreEngine == null)
                throw new ArgumentNullException("KVStoreRedisEngine haven't been setted.");

            KVStoreEntityTable<K, V> table = null;

            if (!ht.ContainsKey(redisKey))
            {
                lock (syncObj)
                {
                    if (!ht.ContainsKey(redisKey))
                    {
                        table = new KVStoreEntityTable<K, V>(redisKey, kvstoreEngine, redisDuration);
                        ht[redisKey] = table;
                    }
                }
            }

            table = ht[redisKey] as KVStoreEntityTable<K, V>;

            return table;
        }
        public static KVStoreEntityTable<K, V> GetKVStoreEntityTable<K, V>(string redisKey, int redisDuration, int dbnum)
        {
            if (kvstoreEngine == null)
                throw new ArgumentNullException("KVStoreRedisEngine haven't been setted.");

            KVStoreEntityTable<K, V> table = null;

            if (!ht.ContainsKey(redisKey))
            {
                lock (syncObj)
                {
                    if (!ht.ContainsKey(redisKey))
                    {
                        table = new KVStoreEntityTable<K, V>(redisKey, kvstoreEngine, redisDuration, dbnum);
                        ht[redisKey] = table;
                    }
                }
            }

            table = ht[redisKey] as KVStoreEntityTable<K, V>;

            return table;
        }

        public static KVStoreList<K, V> GetKVStoreList<K, V>(string redisKey, int redisDuration = 0, int dbnum = 0)
        {
            if (kvstoreEngine == null)
                throw new ArgumentNullException("KVStoreRedisEngine haven't been setted.");
            KVStoreList<K, V> table = null;

            if (!ht.ContainsKey(redisKey))
            {
                lock (syncObj)
                {
                    if (!ht.ContainsKey(redisKey))
                    {
                        table = new KVStoreList<K, V>(redisKey, kvstoreEngine, redisDuration, dbnum);
                        ht[redisKey] = table;
                    }
                }
            }
            table = ht[redisKey] as KVStoreList<K, V>;

            return table;
        }

        public static KVStoreHash<K, F, V> GetKVStoreHash<K, F, V>(string redisKey)
        {
            if (kvstoreEngine == null)
                throw new ArgumentNullException("KVStoreRedisEngine haven't been setted.");
            KVStoreHash<K, F, V> table = null;

            if (!ht.ContainsKey(redisKey))
            {
                lock (syncObj)
                {
                    if (!ht.ContainsKey(redisKey))
                    {
                        table = new KVStoreHash<K, F, V>(redisKey, kvstoreEngine);
                        ht[redisKey] = table;
                    }
                }
            }
            table = ht[redisKey] as KVStoreHash<K, F, V>;

            return table;
        }

        #endregion

        #region [          Property           ]

        public static IKvStoreRedisEngine DefaultEngine
        {
            get
            {
                return kvstoreEngine;
            }
        }

        #endregion
    }
}
