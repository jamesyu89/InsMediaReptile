using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using InstagramPhotos.Utility.Data;

namespace InstagramPhotos.Utility.CommonQuery
{
    public sealed class CommonDataProvider : DataProviderBase
    {
        #region Conn

        protected override SqlConnection GetSqlConnection()
        {
            return null;
        }

        #endregion

        #region Singleton

        public static CommonDataProvider Instance = new CommonDataProvider();

        private CommonDataProvider()
        {
        }

        #endregion

        #region 通用IDs查询方法

        private static SqlParameter GetQuerySqlParamter(QueryParamater qp, Int32 i)
        {
            return GetQuerySqlParamter(qp, qp.QueryValue, i);
        }

        private static SqlParameter GetQuerySqlParamter(QueryParamater qp, string queryValue, Int32 i)
        {
            return new SqlParameter(string.Format("@{0}{1}", qp.QueryName, i), queryValue);
        }

        /// <summary>
        /// 通用IDs查询方法
        /// </summary>
        /// <param name="strSql">查询字符串</param>
        /// <param name="queryPars">查询参数</param>
        /// <param name="connstr">连接字符串</param>
        /// <returns>IDs</returns>
        public List<T> GetIDsByConditions<T>(String strSql, List<QueryParamater> queryPars, SqlConnection connstr)
        {
            //SQL参数序列
            var sqlParsList = BuildSqlParameters(queryPars);

            var IDs = new List<T>();
            using (connstr)
            {
                using (
                    SqlDataReader dr = SqlHelper.ExecuteReader(connstr, strSql, CommandType.Text, sqlParsList.ToArray())
                    )
                {
                    if (dr.HasRows)
                        IDs = SqlHelper.PopulateReadersToIds<T>(dr);
                }
            }
            return IDs;
        }

        public List<T> GetIDsByConditions<T>(String strSql, List<QueryParamater> queryPars, SqlConnection connstr,out Int32 totalCounts) 
        {
            totalCounts = 0;
            //SQL参数序列
            var sqlParsList = BuildSqlParameters(queryPars);

            var result = new List<T>();
            using (connstr)
            {
                using (SqlDataReader dr = SqlHelper.ExecuteReader(connstr, strSql, CommandType.Text, sqlParsList.ToArray()))
                {
                    while (dr.Read())
                    {
                        if (totalCounts == 0 && SqlHelper.ReaderExistsColumn(dr, "TotalDataCount"))
                        {
                            totalCounts = Convert.ToInt32(dr["TotalDataCount"]);
                        }
                        if (dr.HasRows)
                        {
                            T id = (T)dr[0];
                            result.Add(id);
                        }
                    }
                }
            }
            return result;
        }

        private static List<SqlParameter> BuildSqlParameters(List<QueryParamater> queryPars)
        {
            //SQL参数序列
            var sqlParsList = new List<SqlParameter>();
            //遍历查询条件并给SQL参数序列赋值
            for (int i = 0; i < queryPars.Count; i++)
            {
                if (!queryPars[i].IsInnerQuery)
                    switch (queryPars[i].EQueryType)
                    {
                        case QueryParamater.QueryType.BETWEEN:
                        case QueryParamater.QueryType.不包含:
                            var sqlPara2 = new SqlParameter(string.Format("@{0}{1}", queryPars[i].QueryName, i),
                                                           SqlDbType.Xml)
                            {
                                SqlValue =
                                    SqlHelper.ConvertIdsToXML("e", queryPars[i].QueryValue.Split(','))
                            };

                            sqlParsList.Add(sqlPara2);
                            break;

                        case QueryParamater.QueryType.包含:

                            var sqlPara = new SqlParameter(string.Format("@{0}{1}", queryPars[i].QueryName, i),
                                                           SqlDbType.Xml)
                            {
                                SqlValue =
                                    SqlHelper.ConvertIdsToXML("e", queryPars[i].QueryValue.Split(','))
                            };

                            sqlParsList.Add(sqlPara);
                            break;
                        case QueryParamater.QueryType.LIKE:
                            sqlParsList.Add(GetQuerySqlParamter(queryPars[i],
                                                                string.Format("%{0}%", queryPars[i].QueryValue), i));
                            break;
                        case QueryParamater.QueryType.左LIKE:
                            sqlParsList.Add(GetQuerySqlParamter(queryPars[i],
                                                                string.Format("{0}%", queryPars[i].QueryValue), i));
                            break;
                        case QueryParamater.QueryType.右LIKE:
                            sqlParsList.Add(GetQuerySqlParamter(queryPars[i],
                                                                string.Format("%{0}", queryPars[i].QueryValue), i));
                            break;
                        case QueryParamater.QueryType.不为空:
                        case QueryParamater.QueryType.为空:
                        case QueryParamater.QueryType.右括号:
                        case QueryParamater.QueryType.左括号:
                        case QueryParamater.QueryType.或:
                        case QueryParamater.QueryType.升序:
                        case QueryParamater.QueryType.降序:
                        case QueryParamater.QueryType.组合排序:
                            break;
                        default:
                            sqlParsList.Add(GetQuerySqlParamter(queryPars[i], i));
                            break;
                    }
            }
            return sqlParsList;
        }

        #endregion
    }
}