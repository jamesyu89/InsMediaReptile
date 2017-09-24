using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using InstagramPhotos.Utility.Log;

namespace InstagramPhotos.Utility.Data
{
    public class RepositoryBase
    {
        public RepositoryBase()
        {
            //this.dbkey = DbKeyConst.WMS_DBKey;
            this.dbkey = ExtendDBType.InstagramPhotos.ToString();
        }

        public RepositoryBase(ExtendDBType dbType)
        {
            this.dbkey = dbType.ToString();
        }

        public RepositoryBase(string dbKey)
        {
            this.dbkey = dbKey;
        }
        #region K

        #region member

        private const string main_db_key_fmt = "Exfresh_WMS_ServerDB{0}";
        private const string org_db_prefix_fmt = "Exfresh_WMS_DB0{0}.dbo.";

        #endregion

        #region property

        protected string dbkey { get; set; }
        public string OrgDbPrefix { get; set; }

        #endregion

        #endregion

        protected DbConnection GetConn()
        {
            try
            {
                return DBFactory.CreateConnection(dbkey);
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex, "ERROR when create CreateConnection:{0}", dbkey);
                throw;
            }
        }

        public SqlConnection CreateSqlConnection()
        {
            try
            {
                return DBFactory.GetSqlConnection(dbkey);
            }
            catch (Exception ex)
            {
                //Logger.Exception(ex, "Error when create sqlconnection:{0}", dbkey);
                throw;
            }
        }


        //protected void GetDBKey(Int32 server_index, Int32 db_index = 0)
        //{
        //    dbkey = server_index > 0 ? string.Format(main_db_key_fmt, server_index.ToString("00")) : ((ExtendDBType)server_index).ToString();
        //    OrgDbPrefix = db_index == 0 ? String.Empty : string.Format(org_db_prefix_fmt, db_index);
        //}

        protected void GetDBKey(ExtendDBType dbType)
        {
            dbkey = dbType.ToString();
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        protected string GetTableName(string tableName)
        {
            return OrgDbPrefix + tableName;
        }
    }

    /// <summary>
    /// 扩展DB类型
    /// </summary>
    public enum ExtendDBType
    {
        /// <summary>
        /// 默认数据库
        /// </summary>
       [Description("InstagramPhotos")]
        InstagramPhotos = 0,
    }
}
