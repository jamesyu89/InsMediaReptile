using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace InstagramPhotos.Utility.Data
{
    public class DBFactory
    {
        /// <summary>
        /// 创建数据库连接对象DbConnection
        /// </summary>
        /// <param name="dbkey"></param>
        /// <returns></returns>
        public static DbConnection CreateConnection(string dbkey)
        {
            //设置数据库ProviderName
            DbProviderFactory factory = DbProviderFactories.GetFactory(DbProviderName.MSSql);
            DbConnection conn = factory.CreateConnection();
            if (conn != null)
            {
                conn.ConnectionString = GetConnectionStr(dbkey);
                conn.Open();
                return conn;
            }
            return null;
        }

        /// <summary>
        /// 获取数据库连接对象
        /// </summary>
        /// <param name="dbkey"></param>
        /// <returns></returns>
        public static SqlConnection GetSqlConnection(string dbkey)
        {
            return new SqlConnection(GetConnectionStr(dbkey));
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <param name="dbkey"></param>
        /// <returns></returns>
        public static string GetConnectionStr(string dbkey)
        {
            string strSqlConn = "";
            if (ConfigurationManager.AppSettings[dbkey].Length > 50)
            {
                strSqlConn = ConfigurationManager.AppSettings[dbkey];
            }
            else
            {
                strSqlConn = ConfigurationManager.AppSettings[dbkey];
            }
            if (string.IsNullOrEmpty(dbkey))
            {
                
            }
            return strSqlConn;
        }
    }
}
