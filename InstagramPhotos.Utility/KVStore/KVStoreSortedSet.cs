using System;
using InstagramPhotos.Utility.Configuration;
using InstagramPhotos.Utility.Log;

namespace InstagramPhotos.Utility.KVStore
{
    public class KVStoreSortedSet<K, V>
    {
        #region [           Members           ]

        readonly string _redisKey;
        private readonly int _dbNum = AppSettings.GetValue("redis_default_dbnum", 0);

        IKvStoreRedisEngine engine;

        #endregion

        #region [             Ctor.           ]

        internal KVStoreSortedSet(string redisKey, IKvStoreRedisEngine engine)
        {
            this._redisKey = redisKey;
            this.engine = engine;
        }

        #endregion
        #region [             SortedSet           ]
        private string GetRedisKey(K key)
        {
            return string.Format("{0}:{1}", _redisKey, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool SortedSetAdd(K key, V value,double score)
        {
            try
            {
                engine.SortedSetAdd(GetRedisKey(key), _dbNum, value, score);
                return true;
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex, "Redis框架级错误_SortedSet_1！");
                return false;
            }
        }

        #endregion
    }
}
