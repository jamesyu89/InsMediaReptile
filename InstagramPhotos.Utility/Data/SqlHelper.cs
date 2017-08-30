using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace InstagramPhotos.Utility.Data
{
    public static class SqlHelper
    {
        #region Execute With ParamCache


        /// <summary>
        /// 执行存储过程并返回SqlDataReader
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="parameterValues">排序的参数列表</param>
        /// <returns>结果SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(SqlConnection conn, String name, params object[] parameterValues)
        {
            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(conn.ConnectionString, name);
            AssignParameterValues(parameters, parameterValues);

            return ExecuteReader(conn, name, parameters);
        }

        /// <summary>
        /// 执行存储过程并返回执行结果的第一行第一列
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="parameterValues">排序的参数列表</param>
        /// <returns>执行结果的第一行第一列</returns>
        public static object ExecuteScalar(SqlConnection conn, String name, params object[] parameterValues)
        {
            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(conn.ConnectionString, name);
            AssignParameterValues(parameters, parameterValues);

            return ExecuteScalar(conn, name, parameters);
        }

        /// <summary>
        /// 执行存储过程并返回受影响行数
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="parameterValues">排序的参数列表</param>
        /// <returns>受影响行数</returns>
        public static long ExecuteNonQuery(SqlConnection conn, String name, params object[] parameterValues)
        {
            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(conn.ConnectionString, name);
            AssignParameterValues(parameters, parameterValues);

            return ExecuteNonQuery(conn, name, parameters);
        }

        /// <summary>
        /// 执行存储过程并返回受影响行数，同时获得1个Output参数的值
        /// </summary>
        /// <typeparam name="T">Output参数的CLR类型</typeparam>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="outparam">第一个参数，即output参数</param>
        /// <param name="parameterValues">其余的参数列表</param>
        /// <returns>受影响行数</returns>
        public static long ExecuteNonQuery<T>(SqlConnection conn, String name, out T outparam, params object[] parameterValues)
        {
            outparam = default(T);

            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(conn.ConnectionString, name);
            ArrayList paramValueList = new ArrayList();
            paramValueList.Add(outparam);
            paramValueList.AddRange(parameterValues);
            AssignParameterValues(parameters, paramValueList.ToArray());

            long returnValue = ExecuteNonQuery(conn, name, parameters);

            if (parameters[0].Direction == ParameterDirection.InputOutput || parameters[0].Direction == ParameterDirection.Output)
                outparam = (T)parameters[0].Value;

            return returnValue;
        }

        /// <summary>
        /// 执行存储过程并获得存储过程返回值
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="parameterValues">排序的参数列表</param>
        /// <returns>存储过程返回值</returns>
        public static int ExecuteWithReturn(SqlConnection conn, String name, params object[] parameterValues)
        {
            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(conn.ConnectionString, name);
            AssignParameterValues(parameters, parameterValues);

            return ExecuteWithReturn(conn, name, parameters);
        }

        /// <summary>
        /// 执行存储过程并返回DataSet
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="parameterValues">排序的参数列表</param>
        /// <returns>结果DataSet</returns>
        public static DataSet ExecuteDataSet(SqlConnection conn, CommandType type, String name, params object[] parameterValues)
        {
            if (type == CommandType.StoredProcedure)
            {
                SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(conn.ConnectionString, name);
                AssignParameterValues(parameters, parameterValues);
                return ExecuteDataSet(conn, name, parameters);
            }
            else
            {
                return ExecuteDataSet(conn, name, null);
            }
        }

        /// <summary>
        /// 执行存储过程并返回DataSet
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="timeOut">缓存时间</param>
        /// <param name="parameterValues">排序的参数列表</param>
        /// <returns>结果DataSet</returns>
        public static DataSet ExecuteCachedDataset(SqlConnection conn, CommandType type, String name, int timeOut, params object[] parameterValues)
        {
            DataSet data = DatasetCache.GetCashedDataset(conn.Database, name, timeOut, parameterValues);

            if (data == null)
            {
                data = ExecuteDataSet(conn, type, name, parameterValues);
                if (data != null)
                {
                    DatasetCache.SetCachedDataset(conn.Database, name, timeOut, data, parameterValues);
                }
            }

            return data;
        }

        /// <summary>
        /// 执行脚本并返回DataSet
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="timeOut">缓存时间</param>
        /// <param name="parameterValues">排序的参数列表</param>
        /// <returns>结果DataSet</returns>
        public static DataSet ExecuteCachedDataset(SqlConnection conn, String name, int timeOut, params object[] parameterValues)
        {
            DataSet data = DatasetCache.GetCashedDataset(conn.Database, name, timeOut, parameterValues);

            if (data == null)
            {
                data = ExecuteDataSet(conn, CommandType.StoredProcedure, name, parameterValues);
                if (data != null)
                {
                    DatasetCache.SetCachedDataset(conn.Database, name, timeOut, data, parameterValues);
                }
            }

            return data;
        }

        /// <summary>
        /// 这个方法用来给一组参数对象赋值
        /// </summary>
        /// <param name="commandParameters">array of SqlParameters to be assigned values</param>
        /// <param name="parameterValues">array of objects holding the values to be assigned</param>
        public static void AssignParameterValues(SqlParameter[] commandParameters, params object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                //do nothing if we get no data
                return;
            }

            // we must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            //iterate through the SqlParameters, assigning the values from the corresponding position in the 
            //value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                if (parameterValues[i] != null && (commandParameters[i].Direction == ParameterDirection.Input || commandParameters[i].Direction == ParameterDirection.InputOutput))
                {
                    commandParameters[i].Value = parameterValues[i];
                }
                else //Modify yangtm 2012-2-28
                    commandParameters[i].Value = DBNull.Value;
            }
        }

        public static void AssignParameterValues(SqlParameter[] commandParameters, Hashtable parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                //do nothing if we get no data
                return;
            }

            // we must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Count)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            //iterate through the SqlParameters, assigning the values from the corresponding position in the 
            //value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                if (parameterValues[commandParameters[i].ParameterName] != null && (commandParameters[i].Direction == ParameterDirection.Input || commandParameters[i].Direction == ParameterDirection.InputOutput))
                {
                    commandParameters[i].Value = parameterValues[commandParameters[i].ParameterName];
                }
            }
        }

        #endregion

        #region Execute

        private static int commandTimeOut = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["TimeOut"]) ? 100 : int.Parse(System.Configuration.ConfigurationManager.AppSettings["TimeOut"]);

        public static SqlDataReader ExecuteReader(SqlConnection conn, String name, params SqlParameter[] parameters)
        {
            return ExecuteReader(conn, name, CommandType.StoredProcedure, parameters);
        }

        public static SqlDataReader ExecuteReader(SqlConnection conn, String name, CommandType type, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(name, conn))
            {
                command.CommandTimeout = commandTimeOut;
                command.CommandType = type;

                bool canClear = true;
                if (parameters != null)
                    foreach (SqlParameter commandParameter in parameters)
                    {
                        command.Parameters.Add(commandParameter);
                        if (commandParameter.Direction != ParameterDirection.Input)
                            canClear = false;
                    }
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                if (canClear)
                    command.Parameters.Clear();
                return reader;
            }
        }

        public static object ExecuteScalar(SqlConnection conn, String name, params SqlParameter[] parameters)
        {
            return ExecuteScalar(conn, name, CommandType.StoredProcedure, parameters);
        }

        public static object ExecuteScalar(SqlConnection conn, String sql, CommandType type, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(sql, conn))
            {
                command.CommandTimeout = commandTimeOut;
                command.CommandType = type;

                bool canClear = true;
                if (parameters != null)
                    foreach (SqlParameter commandParameter in parameters)
                    {
                        command.Parameters.Add(commandParameter);
                        if (commandParameter.Direction != ParameterDirection.Input)
                            canClear = false;
                    }
                conn.Open();
                object returnValue = command.ExecuteScalar();
                conn.Close();
                if (canClear)
                    command.Parameters.Clear();
                return returnValue;
            }
        }

        public static long ExecuteNonQuery(SqlConnection conn, String name, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(name, conn))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = commandTimeOut;

                bool canClear = true;
                if (parameters != null)
                    foreach (SqlParameter commandParameter in parameters)
                    {
                        command.Parameters.Add(commandParameter);
                        if (commandParameter.Direction != ParameterDirection.Input)
                            canClear = false;
                    }
                conn.Open();
                long returnValue = command.ExecuteNonQuery();
                conn.Close();
                if (canClear)
                    command.Parameters.Clear();
                return returnValue;
            }
        }

        public static int ExecuteWithReturn(SqlConnection conn, String name, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(name, conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = commandTimeOut;

                if (parameters != null)
                    foreach (SqlParameter commandParameter in parameters)
                    {
                        command.Parameters.Add(commandParameter);
                    }
                command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
                conn.Open();
                command.ExecuteNonQuery();
                int returnValue = Convert.ToInt32(command.Parameters["ReturnValue"].Value);
                conn.Close();
                return returnValue;
            }
        }



        public static DataSet ExecuteDataSet(SqlConnection conn, String name, params SqlParameter[] parameters)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(name, conn))
            {
                using (SqlCommand command = new SqlCommand(name, conn))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    DataSet ds = new DataSet();

                    da.SelectCommand = command;
                    da.Fill(ds);

                    return ds;
                }
            }
        }


        public static DataSet ExecuteDataSet(SqlConnection conn,CommandType cmdtype, String name, params SqlParameter[] parameters)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(name, conn))
            {
                using (SqlCommand command = new SqlCommand(name, conn))
                {
                    command.CommandType = cmdtype;
                    command.CommandTimeout = commandTimeOut;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    DataSet ds = new DataSet();

                    da.SelectCommand = command;
                    da.Fill(ds);

                    return ds;
                }
            }
        }
        #endregion

        #region ExecuteNoquery with sqltransaction
        /// <summary>
        /// 执行存储过程并返回受影响行数
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="parameterValues">排序的参数列表</param>
        /// <returns>受影响行数</returns>
        public static long ExecuteNonQuery(SqlConnection conn, SqlTransaction tran, String name, params object[] parameterValues)
        {
            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(conn.ConnectionString, name);
            AssignParameterValues(parameters, parameterValues);

            return ExecuteNonQuery(tran, name, parameters);
        }

        /// <summary>
        /// 执行存储过程并返回受影响行数，同时获得1个Output参数的值
        /// </summary>
        /// <typeparam name="T">Output参数的CLR类型</typeparam>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="outparam">第一个参数，即output参数</param>
        /// <param name="parameterValues">其余的参数列表</param>
        /// <returns>受影响行数</returns>
        public static long ExecuteNonQuery<T>(SqlConnection conn, SqlTransaction tran, String name, out T outparam, params object[] parameterValues)
        {
            outparam = default(T);

            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(conn.ConnectionString, name);
            ArrayList paramValueList = new ArrayList();
            paramValueList.Add(outparam);
            paramValueList.AddRange(parameterValues);
            AssignParameterValues(parameters, paramValueList.ToArray());

            long returnValue = ExecuteNonQuery(tran, name, parameters);

            if (parameters[0].Direction == ParameterDirection.InputOutput || parameters[0].Direction == ParameterDirection.Output)
                outparam = (T)parameters[0].Value;

            return returnValue;
        }

        public static long ExecuteNonQuery(SqlTransaction tran, String name, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(name, tran.Connection))
            {
                command.Transaction = tran;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = commandTimeOut;

                bool canClear = true;
                if (parameters != null)
                    foreach (SqlParameter commandParameter in parameters)
                    {
                        command.Parameters.Add(commandParameter);
                        if (commandParameter.Direction != ParameterDirection.Input)
                            canClear = false;
                    }
                long returnValue = command.ExecuteNonQuery();
                if (canClear)
                    command.Parameters.Clear();
                return returnValue;
            }
        }

        /// <summary>
        /// 执行存储过程并获得存储过程返回值
        /// </summary>
        /// <param name="conn">链接</param>
        /// <param name="name">存储过程名</param>
        /// <param name="parameterValues">排序的参数列表</param>
        /// <returns>存储过程返回值</returns>
        public static int ExecuteWithReturn(SqlConnection conn, SqlTransaction tran, String name, params object[] parameterValues)
        {
            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(conn.ConnectionString, name);
            AssignParameterValues(parameters, parameterValues);

            return ExecuteWithReturn(tran, name, parameters);
        }

        public static int ExecuteWithReturn(SqlTransaction tran, String name, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(name, tran.Connection))
            {
                command.Transaction = tran;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = commandTimeOut;

                if (parameters != null)
                    foreach (SqlParameter commandParameter in parameters)
                    {
                        command.Parameters.Add(commandParameter);
                    }
                command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
                command.ExecuteNonQuery();
                int returnValue = Convert.ToInt32(command.Parameters["ReturnValue"].Value);
                return returnValue;
            }
        }

        public static object ExecuteScalar(SqlTransaction tran, String name, params SqlParameter[] parameters)
        {
            return ExecuteScalar(tran, name, CommandType.StoredProcedure, parameters);
        }

        public static object ExecuteScalar(SqlTransaction tran, String sql, CommandType type, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(sql, tran.Connection))
            {
                command.Transaction = tran;
                command.CommandTimeout = commandTimeOut;
                command.CommandType = type;

                bool canClear = true;
                if (parameters != null)
                    foreach (SqlParameter commandParameter in parameters)
                    {
                        command.Parameters.Add(commandParameter);
                        if (commandParameter.Direction != ParameterDirection.Input)
                            canClear = false;
                    }
                object returnValue = command.ExecuteScalar();
                if (canClear)
                    command.Parameters.Clear();
                return returnValue;
            }
        }

        #endregion

        #region ExecuteReader with Sqltransaction

        public static SqlDataReader ExecuteReader(SqlConnection conn, SqlTransaction tran, String name, params object[] parameterValues)
        {
            SqlParameter[] parameters = SqlHelperParameterCache.GetSpParameterSet(conn.ConnectionString, name);
            AssignParameterValues(parameters, parameterValues);
            return ExecuteReader(tran, name, parameters);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction tran, String name, params SqlParameter[] parameters)
        {
            return ExecuteReader(tran, name, CommandType.StoredProcedure, parameters);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction tran, String name, CommandType type, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(name, tran.Connection))
            {
                command.Transaction = tran;
                command.CommandTimeout = commandTimeOut;
                command.CommandType = type;


                bool canClear = true;
                if (parameters != null)
                    foreach (SqlParameter commandParameter in parameters)
                    {
                        command.Parameters.Add(commandParameter);
                        if (commandParameter.Direction != ParameterDirection.Input)
                            canClear = false;
                    }
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                if (canClear)
                    command.Parameters.Clear();
                return reader;
            }
        }

        #endregion

        #region Create Parameter

        public static SqlParameter CreateNTextInParameter(String name, String s)
        {
            return CreateInParameter(name, SqlDbType.NText,
                s != null ? s.Length : 16, s);
        }

        public static SqlParameter CreateImageInParameter(String name, Byte[] bytes)
        {
            return CreateInParameter(name, SqlDbType.Image,
                bytes != null ? bytes.Length : 16, bytes);
        }

        public static SqlParameter CreateInParameter(String name, SqlDbType datatype, int size, Object value)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = name;
            parameter.Direction = ParameterDirection.Input;
            parameter.SqlDbType = datatype;
            parameter.Size = size;
            parameter.Value = value;
            return parameter;
        }

        public static SqlParameter CreateOutParameter(String name, SqlDbType datatype, int size)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = name;
            parameter.Direction = ParameterDirection.Output;
            parameter.SqlDbType = datatype;
            parameter.Size = size;
            return parameter;
        }

        #endregion

        #region Functions

        public static object IDFromString(String id)
        {
            return id != null && id.Length > 0 ? (object)Int64.Parse(id) : DBNull.Value;
        }

        public static string CleanSearchString(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return null;

            // Do wild card replacements
            searchString = searchString.Replace("*", "%");

            // Strip any markup characters
            //searchString = Transforms.StripHtmlXmlTags(searchString);

            // Remove known bad SQL characters
            searchString = Regex.Replace(searchString, "--|;|'|\"", " ", RegexOptions.Compiled | RegexOptions.Multiline);

            // Finally remove any extra spaces from the string
            searchString = Regex.Replace(searchString, " {1,}", " ", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

            return searchString;
        }

        public static List<int> PopulateReadersToIds(IDataReader dr, string key)
        {
            return PopulateReadersToIds<int>(dr, key);
        }

        public static List<T> PopulateReadersToIds<T>(IDataReader dr, string key)
        {
            List<T> ids = new List<T>();
            //Dictionary<T, bool> existsIds = new Dictionary<T, bool>();
            while (dr.Read())
            {
                T id = (T)dr[key];
                //if (!existsIds.ContainsKey(id))
                //{
                //existsIds.Add(id, true);
                ids.Add(id);
                //}
            }
            return ids;
        }

        public static List<T> PopulateReadersToIds<T>(IDataReader dr)
        {
            List<T> ids = new List<T>();
            while (dr.Read())
            {
                T id = (T)dr[0];
                ids.Add(id);
            }
            return ids;
        }

        public static Dictionary<string, DateTime> PopulateReadersToDic(IDataReader dr, string key, string value)
        {
            return PopulateReadersToDic<string, DateTime>(dr, key, value);
        }

        public static Dictionary<T1, T2> PopulateReadersToDic<T1, T2>(IDataReader dr, string key, string value)
        {
            Dictionary<T1, T2> result = new Dictionary<T1, T2>();

            while (dr.Read())
            {
                T1 name = (T1)dr[key];
                T2 date = (T2)dr[value];

                result.Add(name, date);

            }
            return result;

        }

        public static object ConvertIdsToXML<T>(string itemName, T[] ids)
        {
            string rootName = itemName + "s";
            string idName = "i";
            return ConvertIdsToXML<T>(rootName, itemName, idName, ids);
        }

        public static object ConvertModelListToXML<T>(string itemName, List<T> modelList)
        {
            if (modelList == null)
            {
                return DBNull.Value;
            }

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);
            writer.WriteStartElement(itemName + "s");

            foreach (T model in modelList)
            {
                writer.WriteStartElement(itemName);
                foreach (PropertyInfo p in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    try
                    {
                        writer.WriteAttributeString(p.Name, p.GetValue(model, null) == null ? "" : p.GetValue(model, null).ToString());
                    }
                    catch (Exception)
                    {
                        writer.WriteAttributeString(p.Name, DBNull.Value.ToString());
                    }
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
            return sw.ToString();
        }

        public static object ConvertDicToXML<T, K>(string rootName, Dictionary<T, K> dictionary)
        {
            if (dictionary == null)
            {
                return DBNull.Value;
            }

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);
            writer.WriteStartElement(rootName + "s");
            foreach (var item in dictionary)
            {
                writer.WriteStartElement(rootName);
                writer.WriteAttributeString("k", item.Key.ToString());
                writer.WriteAttributeString("v", item.Value.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
            return sw.ToString();
        }

        public static object ConvertIdsToXML<T>(string rootName, string itemName, string idName, T[] ids)
        {
            if (ids == null)
                return DBNull.Value;

            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);
            writer.WriteStartElement(rootName);
            foreach (T id in ids)
            {
                writer.WriteStartElement(itemName);
                writer.WriteAttributeString(idName, id.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
            return sw.ToString();
        }

        #endregion

        #region SQL TypeSafe

        public static object GetSafeSqlDateTime(DateTime? date)
        {
            if (date == null)
                return DBNull.Value;
            return GetSafeSqlDateTime(date.Value);
        }

        public static DateTime GetSafeSqlDateTime(DateTime date)
        {
            if (date < SqlDateTime.MinValue)
            {
                return (DateTime)SqlDateTime.MinValue;
            }
            if (date > SqlDateTime.MaxValue)
            {
                return (DateTime)SqlDateTime.MaxValue;
            }
            return date;
        }

        public static string GetSafeSqlDateTimeFormat(DateTime date)
        {
            return date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.SortableDateTimePattern);
        }

        public static int GetSafeSqlInt(int i)
        {
            if (i <= ((int)SqlInt32.MinValue))
            {
                return (((int)SqlInt32.MinValue) + 1);
            }
            if (i >= ((int)SqlInt32.MaxValue))
            {
                return (((int)SqlInt32.MaxValue) - 1);
            }
            return i;
        }

        public static object StringOrNull(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return DBNull.Value;
            }
            return text;
        }
        /// <summary>
        /// 判断 DataReader 里面是否包含指定的列
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static bool ReaderExistsColumn(IDataReader dr, string columnName)
        {
            try
            {
                dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
                return (dr.GetSchemaTable().DefaultView.Count > 0);
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
