using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPhotos.Utility.Data
{
    public static class DBTools
    {
        public static int ExecuteNonQuery(DbConnection conn, string commandText, CmdParams cmdParams = null)
        {
            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(DbConnection conn, string commandText, CmdParams cmdParams = null)
        {
            using (DbCommand cmd =await CreateCommandAsync(conn, commandText, cmdParams))
            {
                return  cmd.ExecuteNonQuery();
            }
        }

        public static int ExecuteNonQuery(DbConnection conn, DbTransaction trans, string commandText,
            CmdParams cmdParams = null)
        {
            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            {
                cmd.Transaction = trans;
                return cmd.ExecuteNonQuery();
            }
        }

        public static int ExecuteNonQuery(DbConnection conn, string commandText, ComplexParams cmdParams=null)
        {
            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public static int ExecuteNonQuery(DbConnection conn, DbTransaction trans, string commandText,
            ComplexParams cmdParams = null)
        {
            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            {
                cmd.Transaction = trans;
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(DbConnection conn, string commandText, CmdParams cmdParams)
        {
            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            {
                return cmd.ExecuteScalar();
            }
        }

        public static T ExecuteScalar<T>(DbConnection conn, string commandText, CmdParams cmdParams)
        {
            object result = ExecuteScalar(conn, commandText, cmdParams);
            if (typeof (T) == typeof (Guid))
            {
                if (result == null)
                {
                    return (T) ((object) Guid.Empty);
                }
                return (T) ((object) new Guid(result.ToString()));
            }
            if (result is DBNull || result == null)
            {
                return default(T);
            }
            return (T) Convert.ChangeType(result, typeof (T));
        }

        public static DbDataReader ExecuteReader(DbConnection conn, string commandText, CmdParams cmdParams)
        {
            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            {
                return cmd.ExecuteReader();
            }
        }

        public static DbDataReader ExecuteReader(DbConnection conn, string commandText, ComplexParams cmdParams)
        {
            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            {
                return cmd.ExecuteReader();
            }
        }

        public static T ExecuteReader<T>(DbConnection conn, string commandText, CmdParams cmdParams = null,
            Func<DbDataReader, T> func = null) where T : class
        {
            if (func == null)
                func = (r => r.ToObject<T>());

            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return func(reader);
                }
                return null;
            }
        }

        public static T ExecuteReader<T>(DbConnection conn, string commandText, ComplexParams cmdParams = null,
            Func<DbDataReader, T> func = null) where T : class
        {
            if (func == null)
                func = (r => r.ToObject<T>());

            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return func(reader);
                }
                return null;
            }
        }

        public static T ExecuteReaderStruct<T>(DbConnection conn, string commandText, CmdParams cmdParams,
            Func<DbDataReader, T> func) where T : struct
        {
            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return func(reader);
                }
                return default(T);
            }
        }

        public static IEnumerable<T> ExecuteReaderEnum<T>(DbConnection conn, string commandText, CmdParams cmdParams,
            Func<DbDataReader, T> func)
        {
            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return func(reader);
                }
            }
        }

        public static IEnumerable<T> ReadCollection<T>(
            DbConnection conn,
            string commandText,
            CmdParams cmdParams,
            Func<DbDataReader, T> func = null) where T : class
        {
            return ReadCollection(conn, commandText, cmdParams, null, null, func);
        }

        public static IEnumerable<T> ReadCollection<T>(
            DbConnection conn,
            string commandText,
            CmdParams cmdParams,
            string[] excludeFields,
            Func<DbDataReader, T> func = null)
        {
            return ReadCollection(conn, commandText, cmdParams, excludeFields, null, func);
        }

        public static IEnumerable<T> ReadCollection<T>(
            DbConnection conn,
            string commandText,
            CmdParams cmdParams,
            string[] excludeFields,
            ObjectsChangeTracker changeTracker,
            Func<DbDataReader, T> func = null)
        {
            using (DbCommand cmd = CreateCommand(conn, commandText, cmdParams))
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (func == null)
                    func = (r => r.ToObject<T>(null, excludeFields, changeTracker));
                while (reader.Read())
                {
                    yield return func(reader);
                }
            }
        }

        public static DbCommand CreateCommand(DbConnection conn, string commandText)
        {
            CmdParams dbNull = null;
            DbCommand result = CreateCommand(conn, commandText, dbNull);
            return result;
        }

        public static DbCommand AddParam(this DbCommand cmd, string paramName, object paramValue)
        {
            if (paramValue is Guid)
            {
                paramValue = ((Guid) paramValue).ToGuidStr();
            }

            if (paramValue == null)
            {
                paramValue = DBNull.Value;
            }

            DbParameter par = cmd.CreateParameter();

            par.ParameterName = paramName;
            par.Value = paramValue;
            cmd.Parameters.Add(par);
            return cmd;
        }

        public static DbCommand AddParam(this DbCommand cmd, string paramName, object paramValue, DbType dbType)
        {
            if (paramValue is Guid)
            {
                paramValue = ((Guid) paramValue).ToGuidStr();
            }

            if (paramValue == null)
            {
                paramValue = DBNull.Value;
            }

            DbParameter par = cmd.CreateParameter();
            par.DbType = dbType;
            par.ParameterName = paramName;
            par.Value = paramValue;
            cmd.Parameters.Add(par);
            return cmd;
        }

        public static DbCommand CreateCommand(DbConnection conn, string commandText, CmdParams cmdParams)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            DbCommand result = conn.CreateCommand();
            result.CommandText = commandText;
            result.CommandType = CommandType.Text;
            if (cmdParams != null)
            {
                foreach (var param in cmdParams)
                {
                    object value;
                    if (param.Value is Guid)
                    {
                        value = ((Guid) param.Value).ToGuidStr();
                    }
                    else if (param.Value is bool)
                    {
                        value = ((bool) param.Value).ToShort();
                    }
                    else
                    {
                        value = param.Value;
                    }
                    result.AddParam(param.Key, value);
                }
            }

            return result;
        }

        public static   Task<DbCommand> CreateCommandAsync(DbConnection conn, string commandText, CmdParams cmdParams)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            DbCommand result = conn.CreateCommand();
            result.CommandText = commandText;
            result.CommandType = CommandType.Text;
            if (cmdParams != null)
            {
                foreach (var param in cmdParams)
                {
                    object value;
                    if (param.Value is Guid)
                    {
                        value = ((Guid)param.Value).ToGuidStr();
                    }
                    else if (param.Value is bool)
                    {
                        value = ((bool)param.Value).ToShort();
                    }
                    else
                    {
                        value = param.Value;
                    }
                    result.AddParam(param.Key, value);
                }
            }

             return  Task.FromResult(result);
        }

        public static DbCommand CreateCommand(DbConnection conn, CommandType type, string commandText,
            CmdParams cmdParams)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            DbCommand result = conn.CreateCommand();
            result.CommandText = commandText;
            result.CommandType = type;
            if (cmdParams != null)
            {
                foreach (var param in cmdParams)
                {
                    object value;
                    if (param.Value is Guid)
                    {
                        value = ((Guid) param.Value).ToGuidStr();
                    }
                    else if (param.Value is bool)
                    {
                        value = ((bool) param.Value).ToShort();
                    }
                    else
                    {
                        value = param.Value;
                    }
                    result.AddParam(param.Key, value);
                }
            }

            return result;
        }

        public static DbCommand CreateCommand(DbConnection conn, string commandText, ComplexParams cmdParams)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            DbCommand result = conn.CreateCommand();
            result.CommandText = commandText;
            result.CommandType = CommandType.Text;
            if (cmdParams != null)
            {
                foreach (ComplexParameter param in cmdParams)
                {
                    object value;
                    if (param.Value is Guid)
                    {
                        value = ((Guid) param.Value).ToGuidStr();
                    }
                    else if (param.Value is bool)
                    {
                        value = ((bool) param.Value).ToShort();
                    }
                    else
                    {
                        value = param.Value;
                    }
                    result.AddParam(param.Key, value, param.DbType);
                }
            }

            return result;
        }

        public static DbCommand CreateStoredProcedureCommand(DbConnection conn, string spName)
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            DbCommand result = conn.CreateCommand();
            result.CommandText = spName;
            result.CommandType = CommandType.StoredProcedure;
            return result;
        }

        public static int ExecuteStoredProcedure(DbConnection conn, string commandText, CmdParams cmdParams = null)
        {
            using (DbCommand cmd = CreateCommand(conn, CommandType.StoredProcedure, commandText, cmdParams))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public static DbCommand CreateStoredProcedureCommand(DbConnection conn, string spName, CmdParams cmdParams)
        {
            using (DbCommand cmd = CreateCommand(conn, CommandType.StoredProcedure, spName, cmdParams))
            {
                return cmd;
            }
        }

        public static string ToCSV<T>(this IEnumerable<T> collection, string delim)
        {
            if (collection == null)
            {
                return "";
            }

            var result = new StringBuilder();
            foreach (T value in collection)
            {
                result.Append(value);
                result.Append(delim);
            }
            if (result.Length > 0)
            {
                result.Length -= delim.Length;
            }
            return result.ToString();
        }

        public static T ToObject<T>(this DbDataReader reader)
        {
            return reader.ToObject<T>(null, null, null);
        }

        public static T ToObject<T>(this DbDataReader reader, string readerName)
        {
            return reader.ToObject<T>(readerName, null, null);
        }

        public static T ToObject<T>(this DbDataReader reader, string[] excludeFields)
        {
            return reader.ToObject<T>(null, excludeFields, null);
        }

        public static T ToObject<T>(this DbDataReader reader, string readerName, string[] excludeFields,
            ObjectsChangeTracker changeTracker)
        {
            T result = new DataReaderToObjectMapper<T>(readerName, null, excludeFields).MapUsingState(reader, reader);
            if (changeTracker != null)
            {
                changeTracker.RegisterObject(result);
            }
            return result;
        }

        public static IEnumerable<T> ToObjects<T>(this DbDataReader reader)
        {
            return reader.ToObjects<T>(null, null, null);
        }

        public static IEnumerable<T> ToObjects<T>(this DbDataReader reader, string readerName)
        {
            return reader.ToObjects<T>(readerName, null, null);
        }

        public static IEnumerable<T> ToObjects<T>(this DbDataReader reader, string[] excludeFields)
        {
            return reader.ToObjects<T>(null, excludeFields, null);
        }

        public static IEnumerable<T> ToObjects<T>(this DbDataReader reader, string readerName, string[] excludeFields,
            ObjectsChangeTracker changeTracker)
        {
            if (string.IsNullOrEmpty(readerName))
            {
                var mappingKeyBuilder = new StringBuilder();
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    mappingKeyBuilder.Append(reader.GetName(i));
                    mappingKeyBuilder.Append(' ');
                }
                readerName = mappingKeyBuilder.ToString();
            }
            return new DataReaderToObjectMapper<T>(readerName, null, excludeFields).ReadCollection(reader, changeTracker);
        }

        public static object InsertObject(
            DbConnection conn,
            object obj,
            string tableName,
            DbSettings dbSettings,
            string[] includeFields,
            string[] excludeFields = null,
            Boolean isOutGuid=false,
            string outKey=""
            )
        {
            using (DbCommand cmd = conn.CreateCommand())
            {
                cmd.BuildInsertCommand(obj, tableName, dbSettings, includeFields, excludeFields,isOutGuid,outKey);
                var a= cmd.ExecuteScalar();
                return a;
            }
        }

        public static object InsertObjectWithTrans(
            DbConnection conn,
            object obj,
            string tableName,
            DbSettings dbSettings,
            DbTransaction transaction,
            string[] includeFields,
            string[] excludeFields = null,
            Boolean isOutGuid=false,string outKey=""
            )
        {
            using (DbCommand cmd = conn.CreateCommand())
            {
                cmd.BuildInsertCommand(obj, tableName,dbSettings, includeFields, excludeFields,isOutGuid,outKey);
                cmd.Transaction = transaction;
                return cmd.ExecuteScalar();
            }
        }


        public static object InsertObject(
            DbConnection conn,
            object obj,
            string tableName, 
            DbSettings dbSettings,
            Boolean isOutGuid=false,string outKey=""
            )
        {
            using (DbCommand cmd = conn.CreateCommand())
            {
                cmd.BuildInsertCommand(obj, tableName,dbSettings,isOutGuid,outKey);
                return cmd.ExecuteScalar();
            }
        }

        public static void UpdateObject(
            DbConnection conn,
            object obj,
            string tableName,
            string[] idFieldNames,
            ObjectsChangeTracker changeTracker,
            DbSettings dbSettings
            )
        {
            UpdateObject(conn, obj, tableName, idFieldNames, null, null, changeTracker, dbSettings);
        }

        public static void UpdateObject(
            DbConnection conn,
            object obj,
            string tableName,
            string[] idFieldNames,
            string[] includeFields,
            string[] excludeFields,
            ObjectsChangeTracker changeTracker,
            DbSettings dbSettings
            )
        {
            using (DbCommand cmd = conn.CreateCommand())
            {
                if (
                    cmd.BuildUpdateCommand(
                        obj,
                        tableName,
                        idFieldNames,
                        includeFields,
                        excludeFields,
                        changeTracker,
                        dbSettings
                        )
                    )
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static int UpdateObjectWithTrans(
            DbConnection conn,
            object obj,
            string tableName,
            string[] idFieldNames,
            string[] includeFields,
            string[] excludeFields,
            DbSettings dbSettings,
            DbTransaction transaction
            )
        {
            using (DbCommand cmd = conn.CreateCommand())
            {
                cmd.BuildUpdateCommand(obj, tableName, idFieldNames, includeFields, excludeFields, null, dbSettings);
                cmd.Transaction = transaction;
                return cmd.ExecuteNonQuery();
            }
        }

        public static int UpdateObjectWithTrans(DbConnection conn, object obj, string tableName,
            string[] idFieldNames, DbSettings dbSettings, DbTransaction transaction)
        {
            return UpdateObjectWithTrans(conn, obj, tableName, idFieldNames, null, null, dbSettings, transaction);
        }


        public static void UpdateObject(
            DbConnection conn,
            object obj,
            string tableName,
            string[] idFieldNames,
            DbSettings dbSettings
            )
        {
            UpdateObject(conn, obj, tableName, idFieldNames, null, null, null, dbSettings);
        }
    }
}