using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace InstagramPhotos.Utility.Data
{
    /// <summary>
    /// SqlHelperParameterCache provides functions to leverage a static cache of procedure parameters, and the
    /// ability to discover parameters for stored procedures at run-time.
    /// SqlHelperParameterCache支持函数来实现静态缓存存储过程参数，并支持在运行时得到存储过程的参数
    /// </summary>
    public sealed class SqlHelperParameterCache
    {
        #region private methods, variables, and constructors

        //Since this class provides only static methods, make the default constructor private to prevent 
        //instances from being created with "new SqlHelperParameterCache()".
        //类提供的都是静态方法，将默认构造函数设置为私有的以便阻止利用"new SqlHelperParameterCache()"来实例化类
        private SqlHelperParameterCache() { }

        //存储过程参数缓存导HashTable中
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// resolve at run time the appropriate set of SqlParameters for a stored procedure
        /// 在运行时得到一个存储过程的一系列参数信息
        /// </summary>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="connectionString">一个连接对象的有效连接串</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="spName">存储过程名</param>
        /// <param name="includeReturnValueParameter">是否有返回值参数</param>
        /// <returns>参数对象数组，存储过程的所有参数信息</returns>
        private static SqlParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(spName, cn))
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                //从 SqlCommand 指定的存储过程中检索参数信息，并填充指定的 SqlCommand 对象的 Parameters 集。
                SqlCommandBuilder.DeriveParameters(cmd);

                if (!includeReturnValueParameter)
                {
                    //移除第一个参数对象，因为没有返回值，而默认情况下，第一个参数对象是返回值
                    cmd.Parameters.RemoveAt(0);
                }

                SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count]; ;

                cmd.Parameters.CopyTo(discoveredParameters, 0);

                return discoveredParameters;
            }
        }

        //deep copy of cached SqlParameter array
        //复制缓存参数数组（克隆）
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        #endregion

        #region caching functions

        /**/
        /// <summary>
        /// 将参数数组添加到缓存中
        /// </summary>
        /// <param name="connectionString">有效的连接串</param>
        /// <param name="commandText">一个存储过程名或者T-SQL命令</param>
        /// <param name="commandParameters">一个要被缓存的参数对象数组</param>
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            string hashKey = connectionString + ":" + commandText;

            paramCache[hashKey] = commandParameters;
        }

        /**/
        /// <summary>
        /// 从缓存中获得参数对象数组
        /// </summary>
        /// <param name="connectionString">有效的连接串</param>
        /// <param name="commandText">一个存储过程名或者T-SQL命令</param>
        /// <returns>一个参数对象数组</returns>
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = connectionString + ":" + commandText;

            SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];

            if (cachedParameters == null)
            {
                return null;
            }
            else
            {
                return CloneParameters(cachedParameters);
            }
        }

        #endregion

        #region Parameter Discovery Functions

        /**/
        /// <summary>
        /// 获得存储过程的参数集
        /// </summary>
        /// <remarks>
        /// 这个方法从数据库中获得信息，并将之存储在缓存，以便之后的使用
        /// </remarks>
        /// <param name="connectionString">有效的连接串</param>
        /// <param name="commandText">一个存储过程名或者T-SQL命令</param>
        /// <returns>一个参数对象数组</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /**/
        /// <summary>
        /// 获得存储过程的参数集
        /// </summary>
        /// <remarks>
        /// 这个方法从数据库中获得信息，并将之存储在缓存，以便之后的使用
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="spName">the name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>an array of SqlParameters</returns>
        /// <param name="connectionString">有效的连接串</param>
        /// <param name="commandText">一个存储过程名</param>
        /// /// <param name="includeReturnValueParameter">是否有返回值参数</param>
        /// <returns>一个参数对象数组</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            string hashKey = connectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");

            SqlParameter[] cachedParameters;

            cachedParameters = (SqlParameter[])paramCache[hashKey];

            if (cachedParameters == null)
            {
                cachedParameters = (SqlParameter[])(paramCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
            }

            return CloneParameters(cachedParameters);
        }

        #endregion Parameter Discovery Functions
    }
}
