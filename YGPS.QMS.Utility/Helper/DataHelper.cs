using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;

namespace InstagramPhotos.Utility.Helper
{
    public static class DataHelper
    {
        public static Hashtable CreateReturnTable<T>(IEnumerable<T> list, int totalRecord)
        {
            Hashtable htReturnTable = new Hashtable
            {
                {"data",list},
                {"total",totalRecord}
            };
            return htReturnTable;
        }

        public static Hashtable CreateReturnTable(DataTable dt, int totalRecord)
        {
            Hashtable htReturnTable = new Hashtable
            {
                {"data",dt},
                {"total",totalRecord}
            };
            return htReturnTable;
        }
        /// <summary>
        /// Linq结果表达式转DataTable
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="query">结果表达式</param>
        /// <returns></returns>
        public static DataTable CreateReturnTable<T>(IEnumerable<T> query)
        {
            DataTable tbl = new DataTable();
            PropertyInfo[] props = null;
            foreach (T item in query)
            {
                if (props == null)
                {
                    Type t = item.GetType();
                    props = t.GetProperties();
                    foreach (PropertyInfo pi in props)
                    {
                        Type colType = pi.PropertyType;
                        if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        tbl.Columns.Add(pi.Name, colType);
                    }
                }
                DataRow row = tbl.NewRow();
                foreach (PropertyInfo pi in props)
                {

                    row[pi.Name] = pi.GetValue(item, null) ?? DBNull.Value;
                }
                tbl.Rows.Add(row);
            }
            return tbl;
        }

        public static bool CheckIsExsitsKeyInHashtable(DictionaryEntry entry, out string flag, params string[] key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                if (entry.Key.ToString().ToLower().Trim().Contains(key[i].ToLower()))
                {
                    flag = key[i];
                    return true;
                }
            }
            flag = entry.Key.ToString();
            return false;
        }

        public static string InsertField(Hashtable ht)
        {
            string sql = "";
            foreach (DictionaryEntry de in ht)
            {
                if (de.Value != null)
                {
                    sql += de.Key.ToString().Trim() + ",";
                }
            }
            return sql.TrimEnd(',');
        }
        public static string InsertValue(Hashtable ht)
        {
            string sql = "";
            foreach (DictionaryEntry de in ht)
            {
                if (de.Value != null)
                {
                    if (de.Value.ToString().ToLower() == "getdate()")
                    {
                        sql += "getdate(),";
                    }
                    else
                    {
                        sql += "@" + de.Key.ToString() + ",";
                    }
                }
            }
            return sql.TrimEnd(',');
        }

        public static string UpdateSet(Hashtable ht)
        {
            string sql = "";
            foreach (DictionaryEntry de in ht)
            {
                if (de.Value != null)
                {
                    if (de.Value.ToString().ToLower() == "getdate()")
                    {
                        sql += de.Key.ToString().Trim() + "=getdate(),";
                    }
                    else
                    {
                        sql += de.Key.ToString().Trim() + "=@" + de.Key.ToString().Trim() + ",";
                    }
                }
            }
            return sql.TrimEnd(',');
        }

        #region
        /// <summary>
        /// DataTable生成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dataTable) where T : class, new()
        {
            if (dataTable == null || dataTable.Rows.Count <= 0) throw new ArgumentNullException("dataTable", "当前对象为null无法生成表达式树");
            Func<DataRow, T> func = dataTable.Rows[0].ToExpression<T>();
            List<T> collection = new List<T>(dataTable.Rows.Count);
            foreach (DataRow dr in dataTable.Rows)
            {
                collection.Add(func(dr));
            }
            return collection;
        }


        /// <summary>
        /// 生成表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static Func<DataRow, T> ToExpression<T>(this DataRow dataRow) where T : class, new()
        {
            if (dataRow == null) throw new ArgumentNullException("dataRow", "当前对象为null 无法转换成实体");
            ParameterExpression parameter = Expression.Parameter(typeof(DataRow), "dr");
            List<MemberBinding> binds = new List<MemberBinding>();
            for (int i = 0; i < dataRow.ItemArray.Length; i++)
            {
                String colName = dataRow.Table.Columns[i].ColumnName;
                PropertyInfo pInfo = typeof(T).GetProperty(colName);
                if (pInfo == null || !pInfo.CanWrite) continue;
                MethodInfo mInfo = typeof(DataRowExtensions).GetMethod("Field", new Type[] { typeof(DataRow), typeof(String) }).MakeGenericMethod(pInfo.PropertyType);
                MethodCallExpression call = Expression.Call(mInfo, parameter, Expression.Constant(colName, typeof(String)));
                MemberAssignment bind = Expression.Bind(pInfo, call);
                binds.Add(bind);
            }
            MemberInitExpression init = Expression.MemberInit(Expression.New(typeof(T)), binds.ToArray());
            return Expression.Lambda<Func<DataRow, T>>(init, parameter).Compile();
        }
        #endregion
        /// <summary>
        /// 生成lambda表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Func<SqlDataReader, T> ToExpression<T>(this SqlDataReader reader) where T : class, new()
        {
            if (reader == null || reader.IsClosed || !reader.HasRows) throw new ArgumentException("reader", "当前对象无效");
            ParameterExpression parameter = Expression.Parameter(typeof(SqlDataReader), "reader");
            List<MemberBinding> binds = new List<MemberBinding>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                String colName = reader.GetName(i);
                PropertyInfo pInfo = typeof(T).GetProperty(colName);
                if (pInfo == null || !pInfo.CanWrite) continue;
                MethodInfo mInfo = reader.GetType().GetMethod("GetFieldValue").MakeGenericMethod(pInfo.PropertyType);
                MethodCallExpression call = Expression.Call(parameter, mInfo, Expression.Constant(i));
                MemberAssignment bind = Expression.Bind(pInfo, call);
                binds.Add(bind);
            }
            MemberInitExpression init = Expression.MemberInit(Expression.New(typeof(T)), binds.ToArray());
            return Expression.Lambda<Func<SqlDataReader, T>>(init, parameter).Compile();
        }
    }

    /// <summary>
    /// DataTable转换为List&lt;Model&gt;
    /// </summary>
    public static class TableModelHelper<T> where T : new()
    {
        public static IList<T> ConvertToModel(DataTable dt)
        {
            //定义集合
            IList<T> ts = new List<T>();
            T t = new T();
            string tempName = "";
            //获取此模型的公共属性
            PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach (DataRow row in dt.Rows)
            {
                t = new T();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    //检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        //判断此属性是否有set
                        if (!pi.CanWrite)
                            continue;
                        object value = row[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 将DataRow赋值给model中同名属性
        /// </summary>
        /// <param name="dtRow">DataTable行数据</param>
        public static TA TableRowToModel<TA>(DataRow dtRow) where TA : new()
        {
            //获取model的类型
            Type modelType = typeof(TA);
            TA objmodel =new TA();
            //获取model中的属性
            PropertyInfo[] modelpropertys = modelType.GetProperties();
            //遍历model每一个属性并赋值DataRow对应的列
            foreach (PropertyInfo pi in modelpropertys)
            {
                //获取属性名称
                String name = pi.Name;
                if (dtRow.Table.Columns.Contains(name))
                {
                    //非泛型
                    if (!pi.PropertyType.IsGenericType)
                    {
                        pi.SetValue(objmodel, string.IsNullOrEmpty(dtRow[name].ToString()) ? null : Convert.ChangeType(dtRow[name], pi.PropertyType), null);
                    }
                    //泛型Nullable<>
                    else
                    {
                        Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                        //model属性是可为null类型，进行赋null值
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            //返回指定可以为 null 的类型的基础类型参数
                            pi.SetValue(objmodel, string.IsNullOrEmpty(dtRow[name].ToString()) ? null : Convert.ChangeType(dtRow[name], Nullable.GetUnderlyingType(pi.PropertyType)), null);
                        }
                    }
                }
            }
            return objmodel;
        }
    }

}
