using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using InstagramPhotos.Utility.Utility;

namespace InstagramPhotos.Utility.Data
{
    public abstract class DataProviderBase
    {
        protected abstract SqlConnection GetSqlConnection();

        protected virtual SqlConnection GetReadOnlySqlConnection()
        {
            throw new NotSupportedException();
        }

        public SqlConnection GetSqlConnection(SqlConnectionType connectionType)
        {
            if (connectionType == SqlConnectionType.ReadWrite)
                return GetSqlConnection();

            return GetReadOnlySqlConnection();
        }

        public virtual SqlConnection CreateSqlConnection()
        {
            return GetSqlConnection();
        }

        public List<T> GetAllFromTable<T>(SqlConnectionType connectionType, string tableName)
            where T : ILoadDr
        {
            var result = new List<T>();

            using (SqlConnection conn = GetSqlConnection(connectionType))
            {
                using (
                    SqlDataReader dr = SqlHelper.ExecuteReader(conn,
                        string.Format("select * from {0} with(nolock)", tableName), CommandType.Text, null))
                {
                    while (dr.Read())
                    {
                        var model = (T) EmitHelper.GetInstanceCreator(typeof (T)).Invoke();
                        model.LoadData(dr);
                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public List<T> GetAllFromTable<T>(string tableName)
            where T : ILoadDr
        {
            return GetAllFromTable<T>(SqlConnectionType.ReadWrite, tableName);
        }

        public List<T> GetAllFromQuery<T>(SqlConnectionType connectionType, string query)
            where T : ILoadDr
        {
            var result = new List<T>();

            using (SqlConnection conn = GetSqlConnection(connectionType))
            {
                using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, query, CommandType.Text, null))
                {
                    while (dr.Read())
                    {
                        var model = (T) EmitHelper.GetInstanceCreator(typeof (T)).Invoke();
                        model.LoadData(dr);
                        result.Add(model);
                    }
                }
            }

            return result;
        }

        public List<T> GetAllFromQuery<T>(string query)
            where T : ILoadDr
        {
            return GetAllFromQuery<T>(SqlConnectionType.ReadWrite, query);
        }

        public T GetIDBySP<T>(SqlConnectionType connectionType, string spName, params object[] parameters)
        {
            List<T> ids = GetIDsBySP<T>(connectionType, spName, parameters);
            if (ids != null && ids.Count > 0)
            {
                return ids[0];
            }
            return default(T);
        }

        public T GetIDBySP<T>(string spName, params object[] parameters)
        {
            return GetIDBySP<T>(SqlConnectionType.ReadWrite, spName, parameters);
        }

        public List<T> GetIDsBySP<T>(SqlConnectionType connectionType, string spName, params object[] parameters)
        {
            var result = new List<T>();

            using (SqlConnection conn = GetSqlConnection(connectionType))
            {
                using (SqlDataReader dr = SqlHelper.ExecuteReader(conn, spName
                    , parameters))
                {
                    result = SqlHelper.PopulateReadersToIds<T>(dr);
                }
            }
            return result;
        }

        public List<T> GetIDsBySP<T>(string spName, params object[] parameters)
        {
            return GetIDsBySP<T>(SqlConnectionType.ReadWrite, spName, parameters);
        }

        public DataTable GetTableFromQuery(SqlConnectionType connectionType, string query)
        {
            using (SqlConnection conn = GetSqlConnection(connectionType))
            {
                DataSet ds = SqlHelper.ExecuteCachedDataset(conn, CommandType.Text, query, 60);
                return ds.Tables[0];
            }
        }

        public DataTable GetTableFromQuery(string query)
        {
            return GetTableFromQuery(SqlConnectionType.ReadWrite, query);
        }

        public DataTable GetTableFromQuery(SqlConnectionType connectionType, string query, params object[] parameters)
        {
            using (SqlConnection conn = GetSqlConnection(connectionType))
            {
                DataSet ds = SqlHelper.ExecuteCachedDataset(conn, CommandType.Text, query, 60, parameters);
                return ds.Tables[0];
            }
        }

        public DataTable GetTableFromQuery(string query, params object[] parameters)
        {
            return GetTableFromQuery(SqlConnectionType.ReadWrite, query, parameters);
        }

        public DataSet GetSetFromQuery(SqlConnectionType connectionType, string query)
        {
            using (SqlConnection conn = GetSqlConnection(connectionType))
            {
                DataSet ds = SqlHelper.ExecuteCachedDataset(conn, CommandType.Text, query, 60);
                return ds;
            }
        }

        public DataSet GetSetFromQuery(string query)
        {
            return GetSetFromQuery(SqlConnectionType.ReadWrite, query);
        }

        public DataSet GetSetFromQuery(SqlConnectionType connectionType, string query, params object[] parameters)
        {
            using (SqlConnection conn = GetSqlConnection(connectionType))
            {
                DataSet ds = SqlHelper.ExecuteCachedDataset(conn, CommandType.Text, query, 60, parameters);
                return ds;
            }
        }

        public DataSet GetSetFromQuery(string query, params object[] parameters)
        {
            return GetSetFromQuery(SqlConnectionType.ReadWrite, query, parameters);
        }
    }
}