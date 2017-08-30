// ***********************************************************************
// Assembly         : Exfresh.WMS.Utility
// Author           : WayneChen
// Created          : 05-06-2015
//
// Last Modified By : WayneChen
// Last Modified On : 05-06-2015
// ***********************************************************************
// <copyright file="KVStoreEntityTable.cs" company="Exfresh">
//     Copyright (c) Exfresh. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using InstagramPhotos.Utility.Configuration;
using InstagramPhotos.Utility.Log;
using InstagramPhotos.Utility.Utility;

namespace InstagramPhotos.Utility.KVStore
{
    //KVstore引擎
    public class KVStoreEntityTable<K, V>
    {
        #region [           Members           ]

        readonly string _redisKey;
        private readonly int _dbNum = AppSettings.GetValue("redis_default_dbnum", 0);

        IKvStoreRedisEngine engine;

        readonly TimeSpan duration;

        #endregion

        #region [             Ctor.           ]

        internal KVStoreEntityTable(string redisKey, IKvStoreRedisEngine engine, int redisSeconds = 0, int dbnum = 0)
        {
            this._redisKey = redisKey;
            this.engine = engine;
            duration = TimeSpan.FromSeconds(redisSeconds);
            if (dbnum != 0)
                _dbNum = dbnum;
        }

        #endregion

        #region [             Cache           ]

        private string GetRedisKey(K key)
        {
            return string.Format("{0}:{1}", _redisKey, key);
        }
        /// <summary>
        /// 新增/覆盖键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddKeyValue(K key, V entity)
        {
            try
            {
                string value;
                if (typeof(V) == typeof(string) || typeof(V) == typeof(int) || typeof(V) == typeof(DateTime))
                { value = entity.ToString(); }
                else
                    value = SerializeHelper.ToJson(entity);
                bool result = engine.SetKey(GetRedisKey(key), value, _dbNum, duration);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_1！");
                return false;
            }
        }
        /// <summary>
        /// 根据key获取value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V GetValue(K key)
        {
            try
            {
                return SerializeHelper.Deserialize<V>(engine.StringGet(GetRedisKey(key), _dbNum));
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_2！");
                return default(V);
            }
        }
        /// <summary>
        /// 根据key获取value(多组key)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IList<V> GetValues(IList<K> key)
        {
            try
            {
                var list = engine.StringGets(key, _dbNum, _redisKey);
                if (list == null || list.Length == 0)
                    return new List<V>();

                return list.Where(o => !string.IsNullOrEmpty(o)).Select(o => JsonConvert.DeserializeObject<V>(o)).ToList();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_2.1！");
                return new List<V>();
            }
        }

        public bool TryGetValue(K key, out V entity)
        {
            try
            {
                entity = default(V);

                if (!engine.ValidKeyHas(GetRedisKey(key), _dbNum))
                    return false;
                entity = GetValue(key);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_4！");
                entity = default(V);
                return false;
            }
        }

        private string[] GetRedisKeys(IList<K> keys)
        {
            if (keys == null || keys.Count == 0)
                return new string[0];
            return keys.Select(o => string.Format("{0}:{1}", _redisKey, o)).ToArray();
        }
        /// <summary>
        /// 根据key删除对应键值对
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(K key)
        {
            try
            {
                bool result = engine.DeleteStringKey(GetRedisKey(key), _dbNum);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_3！");
                return false;
            }
        }

        /// <summary>
        /// 根据key删除对应键值对
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool Remove(List<K> keys)
        {
            if (keys == null || !keys.Any())
                return true;
            try
            {
                var keyList = GetRedisKeys(keys).ToList();
                bool result = engine.DeleteStringKeys(keyList, _dbNum);
                return result;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_3！");
                return false;
            }
        }


        public string[] StringGets<T>(IList<T> keys)
        {
            return engine.StringGets(keys, _dbNum, _redisKey);
        }
        public string[] StringGets(ArrayList keys)
        {
            return engine.StringGets(keys, _dbNum, _redisKey);
        }

        public Dictionary<K, V> Get(IList<K> keys)
        {
            try
            {
                var result = new Dictionary<K, V>();

                var enumerable = keys as K[] ?? keys.ToArray();
                
                foreach (K key in enumerable)
                {
                    V value;
                    if (TryGetValue(key, out value))
                    {
                        if (!result.ContainsKey(key)&&value!=null) //
                            result.Add(key, value);
                    }
                       
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "缓存框架级错误_3！");
                throw;
            }
        }

        public V GetFromDB(K key, Func<K, V> getFromDB)
        {
            V entity;
            if (TryGetValue(key, out entity))
            {
                return entity;
            }
            entity = getFromDB(key);

            if (entity != null)
                AddKeyValue(key, entity);

            return entity;
        }

        public List<V> GetFromDB(IEnumerable<K> keys, Func<IEnumerable<K>, Dictionary<K, V>> getFromDB)
        {
            IEnumerable<K> dit_keys = keys.Distinct();

            var keysToDB = new List<K>();

            var dic = new Dictionary<K, V>();

            var ditKeys = dit_keys as K[] ?? dit_keys.ToArray();
            Dictionary<K, V> exist = Get(ditKeys); //try get by keys from cache

            foreach (K key in ditKeys) //build the result set.
            {
                if (exist.ContainsKey(key))
                    dic.Add(key, exist[key]);
                else
                {
                    keysToDB.Add(key);
                    dic.Add(key, default(V));
                }
            }

            if (keysToDB.Count == 0)
                return dic.Values.ToList();

            Dictionary<K, V> fromdb = getFromDB(keysToDB);

            //save to cache.
            Task.Run(() =>
            {
                foreach (var item in fromdb)
                {
                    if (item.Value != null)
                        AddKeyValue(item.Key, item.Value);
                }
            });

            foreach (var item in fromdb)
            {
                dic[item.Key] = item.Value;
            }

            return dic.Values.ToList();
        }

        public bool UpdateToDB(K key, V entity, Func<V, bool> updateToDB)
        {
            if (updateToDB(entity))
                return AddKeyValue(key, entity);

            return false;
        }
        #endregion

    }
}
