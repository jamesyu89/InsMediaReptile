using System;
using System.Collections;
using System.Data;
using System.Text;

namespace InstagramPhotos.Utility.Data
{
    public sealed class DatasetCache
    {
        internal sealed class CacheEntity
        {
            private DateTime _timestamp;
            private DataSet _data;
            public CacheEntity(DataSet data)
            {
                _timestamp = System.DateTime.Now;
                _data = data;
            }

            public DataSet Data
            {
                get
                {
                    return _data;
                }

                set
                {
                    _timestamp = System.DateTime.Now;
                    _data = value;
                }
            }

            public DateTime TimeStamp
            {
                get
                {
                    return _timestamp;
                }
            }
        }

        private static Hashtable _datasetHash = Hashtable.Synchronized(new Hashtable());

        public static string GetKeyFromSpCommand(string dbkey, string spName, params object[] parameterValues)
        {
            StringBuilder key = new StringBuilder(dbkey.ToLower() + "@" + spName + " ");
            if (parameterValues != null)
                for (int i = 0; i < parameterValues.Length; i++)
                {
                    key.Append(string.Format("@p{0}={1} ", i, parameterValues[i].ToString()));
                }

            return key.ToString();
        }

        public static DataSet GetCashedDataset(string dbkey, string spName, int timeOut, params object[] parameterValues)
        {
            dbkey = dbkey.ToLower();
            string key = GetKeyFromSpCommand(dbkey, spName, parameterValues);
            CacheEntity ce = _datasetHash[key] as CacheEntity;
            if (ce == null) return null;
            if (timeOut <= 0) return ce.Data;

            if (System.DateTime.Now.AddSeconds(0 - timeOut).CompareTo(ce.TimeStamp) < 0)
            {
                return ce.Data;
            }
            else
            {
                return null;
            }
        }

        public static void SetCachedDataset(string dbkey, string spName, int timeOut, DataSet data, params object[] parameterValues)
        {
            string key = GetKeyFromSpCommand(dbkey, spName, parameterValues);
            CacheEntity ce = _datasetHash[key] as CacheEntity;
            if (ce == null)
            {
                ce = new CacheEntity(data);
                _datasetHash.Add(key, ce);
            }
            else
            {
                ce.Data = data;
                _datasetHash[key] = ce;
            }
        }
    }
}
