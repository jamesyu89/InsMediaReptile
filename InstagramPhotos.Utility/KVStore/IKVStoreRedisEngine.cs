// ***********************************************************************
// Assembly         : Exfresh.WMS.Utility
// Author           : WayneChen
// Created          : 05-06-2015
//
// Last Modified By : WayneChen
// Last Modified On : 05-06-2015
// ***********************************************************************
// <copyright file="IKVStoreRedisEngine.cs" company="Exfresh">
//     Copyright (c) Exfresh. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;

namespace InstagramPhotos.Utility.KVStore
{
    //KVstore引擎
    public interface IKvStoreRedisEngine
    {
        CasResult<bool> AddToRedis<T>(KVRedisType type, string key, T value);

        CasResult<bool> AddToRedis<T>(KVRedisType type, string key, string HashField, T value);

        CasResult<bool> AddToRedis<T>(KVRedisType type, string key, T value, int Minutes);

        CasResult<bool> AddToRedis<T>(KVRedisType type, string key, string HashField, T value, int DBNum, int Minutes);

        T GetFromRedis<T>(KVRedisType type, string key);
        T GetFromRedis<T>(KVRedisType type, string key, int DBNum);

        /// <summary>
        /// 递增(int型值,默认为0，在key的值上加1)
        /// </summary>
        CasResult<long> SetKeyInc(string key, int DBNum);

        /// <summary>
        /// 取得string数据
        /// </summary>
        string StringGet(string key, int DBNum);

        /// <summary>
        /// 取得string数据
        /// </summary>
        string[] StringGets<T>(IList<T> keys, int DBNum, string _redisKey);
        /// <summary>
        /// 取得string数据
        /// </summary>
        string[] StringGets(ArrayList keys, int DBNum, string _redisKey);

        /// <summary>
        /// String存入Redis
        /// </summary>
        bool SetKey(string key, string value, int DBNum, TimeSpan? expire);

        /// <summary>
        /// String存入Redis
        /// </summary>
        bool SetKey(string key, string value, int DBNum);

        /// <summary>
        /// String从Redis删除
        /// </summary>
        bool DeleteStringKey(string key, int DBNum);

        /// <summary>
        ///     String从Redis删除
        /// </summary>
        bool DeleteStringKeys(List<string> keys, int DBNum);

        /// <summary>
        /// Hash存入Redis
        /// </summary>
        bool HashSet(string key, string HashField, string value, int DBNum);

        /// <summary>
        /// Hash存入Redis
        /// </summary>
        bool HashSet(string key, Dictionary<string, string> dic, int DBNum);

        /// <summary>
        /// Hash删除
        /// </summary>
        bool DeleteHashSet(string key, ArrayList HashFieldlist, int DBNum);
        /// <summary>
        /// Hash获取
        /// </summary>
        string HashGet(string key, string HashField, int DBNum);


        /// <summary>
        /// Hash获取全部
        /// </summary>
        Dictionary<string, object> HashGetAll(string key, int DBNum);
        /// <summary>
        /// 加入队列
        /// </summary>
        void ListLeftPush(string key, int DBNum, string value);

        void ListLeftPush(string key, int DBNum, string value, TimeSpan? expire);
        /// <summary>
        /// 批量加入队列
        /// </summary>
        void ListLeftPush(string key, int DBNum, string[] value);
        /// <summary>
        /// 从队列中取出
        /// </summary>
        string ListRightPop(string key, int DBNum);

        /// <summary>
        /// 从队列中取出并返回数量
        /// </summary>
        Tuple<long, string> ListRightPopNum(string key, int DBNum);

        /// <summary>
        /// 从队列中取数量
        /// </summary>
        long ListLength(string key, int DBNum);
        /// <summary>
        /// 获取整个列表
        /// </summary>
        IList<T> ListRange<T>(string key, int DBNum);
        /// <summary>
        /// 是否存在Key
        /// </summary>
        bool ValidKeyHas(string key, int DBNum);

        bool SortedSetAdd<T>(string key, int dbNum, T value, double score);
    }

    public enum KVRedisType
    {
        /// <summary>
        /// String key value形式存储
        /// </summary>
        StringSet = 1,
        /// <summary>
        /// Hash形式存储
        /// </summary>
        HashSet = 2,
        /// <summary>
        /// Set（普通List）
        /// </summary>
        Set = 3,
        /// <summary>
        /// List存储（队列，先进先出）
        /// </summary>
        ListPush = 4,
        /// <summary>
        /// 在key上数值递增(+1)
        /// </summary>
        StringIncrement = 5,
        /// <summary>
        /// 获取String 根据key 
        /// </summary>
        StringGet = 6,
        /// <summary>
        /// List取出
        /// </summary>
        ListPop = 7

    }
}
