//using System.Collections.Generic;
//using MongoDB.Driver;
//using Exfresh.WMS.Utility.Configuration;

//namespace Exfresh.WMS.Utility.NoSQL.MongoDB
//{
//    public class MongoDBEngine<T> where T : NoSQLModelBase
//    {
//        private readonly IMongoCollection<T> collection;

//        private readonly string config_mongodb_conn = AppSettings.GetValue("mongodb_conn", "dds-bp126d49ff4073141.mongodb.rds.aliyuncs.com:3717");

//        private readonly string config_mongodb_dbname = AppSettings.GetValue("mongodb_dbname", "FD");
//        public MongoDBEngine(string collectionName)
//        {
//            collection = new MongoClient(config_mongodb_conn).GetDatabase(config_mongodb_dbname).GetCollection<T>(collectionName);
//        }

//        /// <summary>
//        ///     查找
//        /// </summary>
//        /// <param name="filter"></param>
//        /// <returns></returns>
//        public List<T> Find(FilterDefinition<T> filter)
//        {
//            return collection.FindSync(filter).ToList();
//        }

//        public List<T> FindAll()
//        {
//            return collection.FindSync(null).ToList();
//        }

//        /// <summary>
//        ///     修改
//        /// </summary>
//        /// <param name='filter'></param>
//        /// <param name="update"></param>
//        /// <returns></returns>
//        public long Update(FilterDefinition<T> filter, UpdateDefinition<T> update)
//        {
//            var res = collection.UpdateMany(filter, update);
//            return res.ModifiedCount;
//        }

//        /// <summary>
//        ///     添加
//        /// </summary>
//        /// <param name='model'></param>
//        /// <returns></returns>
//        public void Insert(T model)
//        {
//            collection.InsertOne(model);
//        }

//        /// <summary>
//        ///     批量新增
//        /// </summary>
//        /// <param name="models"></param>
//        public void Insert(List<T> models)
//        {
//            collection.InsertMany(models);
//        }

//        /// <summary>
//        ///     批量删除
//        /// </summary>
//        /// <param name='filter'></param>
//        /// <returns></returns>
//        public long Delete(FilterDefinition<T> filter)
//        {
//            var res = collection.DeleteMany(filter);
//            return res.DeletedCount;
//        }
//    }
//}