using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace InstagramPhotos.CodeGender.Helper
{
    public static class SqlHelper
    {

        public static SqlDataReader ExecuteReader(SqlConnection conn, String name, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(name, conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                conn.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
        }

        public static SqlDataReader QueryReader(SqlConnection conn, String name, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(name, conn))
            {
                command.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                conn.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
        }

        public static object ExecuteScalar(SqlConnection conn, String name, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(name, conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                conn.Open();
                object returnValue = command.ExecuteScalar();
                conn.Close();
                return returnValue;
            }
        }

        public static long ExecuteNonQuery(SqlConnection conn, String name, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(name, conn))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30;
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                conn.Open();
                long returnValue = command.ExecuteNonQuery();
                conn.Close();
                return returnValue;
            }
        }

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

        public static object IDFromString(String id)
        {
            return id != null && id.Length > 0 ? (object)Int64.Parse(id) : DBNull.Value;
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

        //public static void BeginSqlQueryExecution(string queryName)
        //{
        //    CommonContext context = CommonContext.Current;
        //    SqlQueryExecution ex = new SqlQueryExecution(queryName);
        //    context.QueryExecutionList[ex.Name] = ex;
        //}

        //public static void EndSqlQueryExecution(string queryName, bool addQueryExecutionCount)
        //{
        //    CommonContext context = CommonContext.Current;
        //    if (addQueryExecutionCount)
        //        context.QueryExecutions++;
        //    if (context.QueryExecutionList.ContainsKey(queryName))
        //        context.QueryExecutionList[queryName].EndTime = DateTime.Now;
        //}


        /// <summary>
        /// 将Id集合变成Xml格式
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public static object ConvertIdsToXML<T>(string itemName, T[] ids)
        {
            string rootName = itemName + "s";
            string idName = "i";
            return ConvertIdsToXML<T>(rootName, itemName, idName, ids);
        }

        /// <summary>
        /// 将Id集合变成Xml格式
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
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
    }
}

