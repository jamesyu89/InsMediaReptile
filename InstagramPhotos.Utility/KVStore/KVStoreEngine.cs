using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using InstagramPhotos.Utility.Configuration;
using InstagramPhotos.Utility.Log;
using InstagramPhotos.Utility.Utility;

namespace InstagramPhotos.Utility.KVStore
{
    //KVstore引擎
    public class KVStoreEngine : IKvStoreRedisEngine
    {
        private static readonly string cal_redis_conn_string = AppSettings.GetValue<string>("CalRedisConnString");

        private readonly Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            //    var configurationOptions = new ConfigurationOptions
            //    {
            //        EndPoints =
            //{
            //    { host, port }
            //},
            //        KeepAlive = 180,
            //        Password = password,
            //        DefaultVersion = new Version("2.8.5"),
            //        // Needed for cache clear
            //        AllowAdmin = true
            //    };

            //    var connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);

            //  ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("endpoint,password=password,ConnectTimeout=10000");
            return ConnectionMultiplexer.Connect(cal_redis_conn_string);
        });


        public ConnectionMultiplexer RedisConnection
        {
            get { return lazyConnection.Value; }
        }

        public CasResult<bool> AddToRedis<T>(KVRedisType type, string key, T value)
        {
            return AddToRedis(type, key, string.Empty, value, 0, 0);
        }

        public CasResult<bool> AddToRedis<T>(KVRedisType type, string key, string HashField, T value)
        {
            return AddToRedis(type, key, HashField, value, 0, 0);
        }

        public CasResult<bool> AddToRedis<T>(KVRedisType type, string key, T value, int Minutes)
        {
            return AddToRedis(type, key, string.Empty, value, 0, Minutes);
        }

        public CasResult<bool> AddToRedis<T>(KVRedisType type, string key, string HashField, T value, int DBNum,
            int Minutes)
        {
            var result = new CasResult<bool>();

            var strValue = SerializeHelper.ToJson(value);
            switch (type)
            {
                case KVRedisType.StringSet:
                    if (Minutes > 0)
                    {
                        var span = new TimeSpan(DateTime.Now.AddMinutes(Minutes).Ticks);
                        result.Result = SetKey(key, strValue, DBNum, span);
                    }
                    else
                    {
                        result.Result = SetKey(key, strValue, DBNum);
                    }
                    break;
                case KVRedisType.HashSet:
                    if (!string.IsNullOrEmpty(HashField))
                        result.Result = HashSet(key, HashField, strValue, DBNum);
                    break;
                case KVRedisType.Set:
                    SetAdd(key, strValue, DBNum);
                    break;
                case KVRedisType.ListPush:
                    ListLeftPush(key, DBNum, strValue);
                    break;
                case KVRedisType.StringIncrement:
                    SetKeyInc(key, DBNum);
                    break;
            }
            return result;
        }

        public T GetFromRedis<T>(KVRedisType type, string key)
        {
            return GetFromRedis<T>(type, key, 0);
        }

        public T GetFromRedis<T>(KVRedisType type, string key, int DBNum)
        {
            try
            {
                switch (type)
                {
                    case KVRedisType.StringGet:

                        return SerializeHelper.Deserialize<T>(StringGet(key, DBNum));

                    case KVRedisType.ListPop:
                        return SerializeHelper.Deserialize<T>(ListRightPop(key, DBNum));
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis反序列化失败");
            }
            return default(T);
        }

        /// <summary>
        ///     递增(int型值,默认为0，在key的值上加1)
        /// </summary>
        public CasResult<long> SetKeyInc(string key, int DBNum)
        {
            var result = new CasResult<long>();

            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                var s = Redis.StringIncrement(key);
                result.Result = s;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis插入递增数值失败");
            }
            return result;
        }

        /// <summary>
        ///     取得string数据
        /// </summary>
        public string StringGet(string key, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                var value = Redis.StringGet(key);
                if (value.IsNullOrEmpty) return "";
                return value.ToString();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis获取string数值失败--key:" + key);
            }
            return "";
        }

        /// <summary>
        ///     取得string数据
        /// </summary>
        public string[] StringGets<T>(IList<T> keys, int DBNum, string _redisKey)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                var arrRedis = new RedisKey[keys.Count];
                var i = 0;
                foreach (var key in keys)
                {
                    arrRedis[i] = string.Format("{0}:{1}", _redisKey, key);
                    i++;
                }
                var list = Redis.StringGet(arrRedis);
                if ((list != null) && (list.Length > 0))
                    return list.ToStringArray();
                return null;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis获取string数值keys失败");
            }
            return null;
        }

        /// <summary>
        ///     取得string数据
        /// </summary>
        public string[] StringGets(ArrayList keys, int DBNum, string _redisKey)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                var arrRedis = new RedisKey[keys.Count];
                var i = 0;
                foreach (var key in keys)
                {
                    arrRedis[i] = string.Format("{0}:{1}", _redisKey, key);
                    i++;
                }
                var list = Redis.StringGet(arrRedis);
                return list.ToStringArray();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis获取string数值keys失败");
            }
            return null;
        }

        /// <summary>
        ///     String存入Redis
        /// </summary>
        public bool SetKey(string key, string value, int DBNum, TimeSpan? expire)
        {
            if ((expire == null) || (expire <= TimeSpan.FromSeconds(0)))
                return SetKey(key, value, DBNum);
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                var result = Redis.StringSet(key, value, expire);
                return result;
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "存入Redis String出错--key：" + key);
            }
            return false;
        }

        /// <summary>
        ///     String存入Redis
        /// </summary>
        public bool SetKey(string key, string value, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                return Redis.StringSet(key, value);
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "存入Redis出错--key：" + key);
            }
            return false;
        }

        /// <summary>
        ///     String从Redis删除
        /// </summary>
        public bool DeleteStringKey(string key, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                return Redis.KeyDelete(key);
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "删除Redis出错--key：" + key);
            }
            return false;
        }

        /// <summary>
        ///     String从Redis删除
        /// </summary>
        public bool DeleteStringKeys(List<string> keys, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                foreach (var key in keys)
                {
                    Redis.KeyDelete(key);
                }
                return true;
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "批量删除Redis出错");
            }
            return false;
        }

        /// <summary>
        ///     Hash存入Redis
        /// </summary>
        public bool HashSet(string key, string HashField, string value, int DBNum)
        {
            // ConnectionMultiplexer conn=    redisConn();
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                Redis.HashSet(key, HashField, value);
                return true;
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "存入Redis出错Hash出错:" + ex.Message);
            }

            return false;
        }


        /// <summary>
        ///     Hash存入Redis
        /// </summary>
        public bool HashSet(string key, Dictionary<string, string> dic, int DBNum)
        {
            if ((dic == null) || (dic.Count == 0))
                return false;
            var hash = new HashEntry[dic.Count];
            var num = 0;
            foreach (var kvp in dic)
            {
                hash[num] = new HashEntry(kvp.Key, kvp.Value);
                num++;
            }
            // ConnectionMultiplexer conn=    redisConn();
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                Redis.HashSet(key, hash);
                return true;
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "存入Redis出错Hash出错:" + ex.Message);
            }
            return false;
        }

        /// <summary>
        ///     Hash删除
        /// </summary>
        public bool DeleteHashSet(string key, ArrayList HashFieldlist, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);

                var arrRedis = (RedisValue[]) HashFieldlist.ToArray(typeof(RedisValue));
                var result = Redis.HashDelete(key, arrRedis);
                if (result > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "删除redis Hash出错key:" + key);
            }
            return false;
        }


        /// <summary>
        ///     Hash获取
        /// </summary>
        public string HashGet(string key, string HashField, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                return Redis.HashGet(key, HashField);
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "读取redis Hash出错key:" + key);
            }
            return "";
        }

        /// <summary>
        ///     Hash获取全部
        /// </summary>
        public Dictionary<string, object> HashGetAll(string key, int DBNum)
        {
            var result = new Dictionary<string, object>();
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                var hash = Redis.HashGetAll(key);
                if ((hash != null) && (hash.Length > 0))
                    foreach (var h in hash)
                        result.Add(h.Name, h.Value);
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "读取redis Hash出错key:" + key);
            }
            return result;
        }

        /// <summary>
        ///     批量加入队列
        /// </summary>
        public void ListLeftPush(string key, int DBNum, string[] value)
        {
            try
            {
                var redisValues = new RedisValue[value.Length];
                for (var i = 0; i < value.Length; i++)
                    redisValues[i] = value[i];
                var Redis = RedisConnection.GetDatabase(DBNum);
                Redis.ListLeftPush(key, redisValues);
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "Redis加入队列出错--key:" + key);
            }
        }

        /// <summary>
        ///     加入队列
        /// </summary>
        public void ListLeftPush(string key, int DBNum, string value, TimeSpan? expire)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                Redis.ListLeftPush(key, value);
                Redis.KeyExpire(key, expire);
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "Redis加入队列出错--key:" + key);
            }
        }

        /// <summary>
        ///     加入队列
        /// </summary>
        public void ListLeftPush(string key, int DBNum, string value)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                Redis.ListLeftPush(key, value);
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "Redis加入队列出错--key:" + key);
            }
        }


        /// <summary>
        ///     从队列中取出
        /// </summary>
        public string ListRightPop(string key, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                var value = Redis.ListRightPop(key);

                if (value.IsNullOrEmpty) return "";
                return value.ToString();
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "Redis取出队列出错--key:" + key);
            }
            return "";
        }

        /// <summary>
        ///     从队列中取出并返回剩余数量
        /// </summary>
        public Tuple<long, string> ListRightPopNum(string key, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                var value = Redis.ListRightPop(key);
                var count = Redis.ListLength(key);

                if (value.IsNullOrEmpty) return new Tuple<long, string>(0, "");
                return new Tuple<long, string>(count, value.ToString());
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "Redis取出队列出错2--key:" + key);
            }
            return new Tuple<long, string>(0, "");
        }


        /// <summary>
        ///     从队列中取数量
        /// </summary>
        public long ListLength(string key, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                var count = Redis.ListLength(key);

                return count;
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "Redis取队列数量出错--key:" + key);
            }
            return 0;
        }

        /// <summary>
        ///     获取整个列表
        /// </summary>
        public IList<T> ListRange<T>(string key, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                var value = Redis.ListRange(key);
                if (value.Length <= 0) return new List<T>();
                return value.Select(o => SerializeHelper.Deserialize<T>(o)).ToList();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Redis取出队列出错--key:" + key);
                return new List<T>();
            }
        }


        /// <summary>
        ///     是否存在Key
        /// </summary>
        public bool ValidKeyHas(string key, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                if (Redis.StringGet(key).HasValue)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "读取Redis是否存在出错--key：" + key);
                return false;
            }
        }

        public bool SortedSetAdd<T>(string key, int DBNum, T value, double score)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                return Redis.SortedSetAdd(key, SerializeHelper.ToJson(value), score);
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "存入Redis Set出错--key：" + key);
            }
            return false;
        }

        /// <summary>
        ///     String存入Redis
        /// </summary>
        public bool SetAdd(string key, string value, int DBNum)
        {
            try
            {
                var Redis = RedisConnection.GetDatabase(DBNum);
                return Redis.SetAdd(key, value);
            }
            catch (Exception ex)
            {
                //记录日志 
                Logger.Exception(ex, "存入Redis Set出错--key：" + key);
            }
            return false;
        }

        }
}