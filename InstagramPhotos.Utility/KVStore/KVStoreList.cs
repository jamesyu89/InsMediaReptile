// ***********************************************************************
// Assembly         : Exfresh.WMS.Utility
// Author           : WayneChen
// Created          : 05-06-2015
//
// Last Modified By : WayneChen
// Last Modified On : 05-06-2015
// ***********************************************************************
// <copyright file="KVStoreList.cs" company="Exfresh">
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
    public class KVStoreList<K, V>
    {
        #region [           Members           ]

        readonly string _redisKey;
        private readonly int _dbNum = AppSettings.GetValue("redis_default_dbnum", 0);

        IKvStoreRedisEngine engine;
        readonly TimeSpan? duration;
        #endregion

        #region [             Ctor.           ]

        internal KVStoreList(string redisKey, IKvStoreRedisEngine engine, int redisSeconds = 0, int dbnum = 0)
        {
            if (redisSeconds > 0)
            {
                duration = TimeSpan.FromSeconds(redisSeconds);
            }
            else
            {
                duration = null;
            }
            if (dbnum != 0)
                _dbNum = dbnum;
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
        /// 批量加入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool PushList(K key, IList<V> list)
        {
            try
            {
                var strList = new List<string>();
                foreach (var entity in list)
                {
                    strList.Add(SerializeHelper.ToJson(entity));
                }
                engine.ListLeftPush(GetRedisKey(key), _dbNum, strList.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_队列_1！");
                return false;
            }
        }
        /// <summary>
        /// 加入队列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool PushList(K key, V entity)
        {
            try
            {
                string value = SerializeHelper.ToJson(entity);
                if (duration != null)
                {
                    engine.ListLeftPush(GetRedisKey(key), _dbNum, value, duration);
                }
                else
                {
                    engine.ListLeftPush(GetRedisKey(key), _dbNum, value);
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_队列_2！");
                return false;
            }
        }



        /// <summary>
        /// 根据key从队列中取出数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V PopList(K key)
        {
            try
            {
                string retVal = GetRedisKey(key);
                if (string.IsNullOrEmpty(retVal))
                {
                    return default(V);
                }
                return SerializeHelper.Deserialize<V>(engine.ListRightPop(GetRedisKey(key), _dbNum));
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_队列_3！");
                return default(V);
            }
        }
        /// <summary>
        /// 从队列中取出并返回剩余数量
        /// </summary>
        public Tuple<long, V> PopListGetNum(K key)
        {
            try
            {
                string retVal = GetRedisKey(key);
                if (string.IsNullOrEmpty(retVal))
                {
                    return new Tuple<long, V>(0, default(V));
                }
                var temp = engine.ListRightPopNum(GetRedisKey(key), _dbNum);

                return new Tuple<long, V>(temp.Item1, SerializeHelper.Deserialize<V>(temp.Item2));
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_队列_4！");
                return new Tuple<long, V>(0, default(V));
            }
        }

        public long GetListNum(K key)
        {
            try
            {
                string retVal = GetRedisKey(key);
                if (string.IsNullOrEmpty(retVal))
                {
                    return 0;
                }
                return engine.ListLength(GetRedisKey(key), _dbNum);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_队列_5！");
                return 0;
            }
        }


        /// <summary>
        /// 获取整个列表
        /// </summary>
        public IList<T> ListRange<T>(K key)
        {
            try
            {
                return engine.ListRange<T>(GetRedisKey(key), _dbNum);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis框架级错误_队列_6！");
                throw;
            }
        }
        #endregion
    }
}
