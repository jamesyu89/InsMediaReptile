// ***********************************************************************
// Assembly         : Exfresh.WMS.Utility
// Author           : WayneChen
// Created          : 05-06-2015
//
// Last Modified By : WayneChen
// Last Modified On : 05-06-2015
// ***********************************************************************
// <copyright file="KVStoreHash.cs" company="Exfresh">
//     Copyright (c) Exfresh. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using InstagramPhotos.Utility.Configuration;
using InstagramPhotos.Utility.Log;
using InstagramPhotos.Utility.Utility;

namespace InstagramPhotos.Utility.KVStore
{
    public class KVStoreHash<K, F, V>
    {
        #region [           Members           ]

        readonly string _redisKey;
        private readonly int _dbNum = AppSettings.GetValue("redis_default_dbnum", 0);

        IKvStoreRedisEngine engine;
     
        #endregion

        #region [             Ctor.           ]

        internal KVStoreHash(string redisKey, IKvStoreRedisEngine engine)
        {
            this._redisKey = redisKey;
            this.engine = engine;
        }

        #endregion

        #region [             Cache           ]

        private string GetRedisKey(K key)
        {
            return string.Format("{0}:{1}", _redisKey, key);
        }
        /// <summary>
        /// 加入Hash
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool HashSet(K key, F field, V entity)
        {
            try
            {
                string strField = string.Format("{0}", field);
                string value = SerializeHelper.ToJson(entity);
                engine.HashSet(GetRedisKey(key), strField, value, _dbNum);
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex, "Redis框架级错误_Hash_1！");
                return false;
            }
        }
        public bool HashSet(K key, Dictionary<K, F> dic, int DBNum)
        {
            Dictionary<string, string> dic_new = new Dictionary<string, string>();

            try
            {
                foreach (var kvp in dic)
                {
                    dic_new.Add(kvp.Key.ToString(), SerializeHelper.ToJson(kvp.Value));
                }
                engine.HashSet(GetRedisKey(key), dic_new, _dbNum);
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex, "Redis框架级错误_Hash_2！");
                return false;
            }
        }
        /// <summary>
        /// 根据key和field从Hash中取出数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V HashGet(K key, F field)
        {
            try
            {
                string strField = string.Format("{0}", field);
                return SerializeHelper.Deserialize<V>(engine.HashGet(GetRedisKey(key), strField, _dbNum));
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex, "Redis框架级错误_Hash_3！");
                throw;
            }
        }

        /// <summary>
        /// 根据key和field从Hash中取出数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, V> HashGet(K key)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Dictionary<string, V> result = new Dictionary<string, V>();
            try
            {
                dic = engine.HashGetAll(GetRedisKey(key), _dbNum);
                foreach (var d in dic)
                {
                    result.Add(d.Key, SerializeHelper.Deserialize<V>(d.Value.ToString()));
                }
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex, "Redis框架级错误_Hash_4！");
                throw;
            }
            return result;

        }
        #endregion


    }
}
