/*----------------------------------------
Developed by:WayneChen(chenwei@ccjoy-inc.com)
Date Created:2015-07-24
Last Updated:2015-07-24
Copyright:CCJoy
Description:通用查询实体参数处理
----------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramPhotos.Utility.CommonQuery
{
    /// <summary>
    ///     通用查询参数实体
    /// </summary>
    [Serializable]
    public class QueryParamater
    {
        /// <summary>
        ///     查询条件间的关系类型
        /// </summary>
        public enum QueryRelatedType
        {
            Null = 0,

            AND = 1,

            OR = 2,

            EMPTY = 3
        }

        public enum QueryType
        {
            等于 = 1,

            BETWEEN = 2,

            大于等于 = 3,

            小于等于 = 4,

            大于 = 5,

            小于 = 6,

            不等于 = 7,

            包含 = 8,

            不包含 = 9,

            LIKE = 11,

            左LIKE = 12,

            右LIKE = 13,

            不为空 = 14,

            为空 = 15,

            异或与 = 20,

            非异或与 = 21,

            升序 = 22,

            降序 = 23,

            组合排序 = 24,

            左括号 = 16,

            右括号 = 17,

            或 = 18
        }

        public QueryParamater()
        {
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="queryName">查询字段名</param>
        /// <param name="queryValue">字段值</param>
        /// <param name="queryType">查询方式</param>
        /// <param name="queryRelatedType">查询条件之间的关系类型枚举</param>
        public QueryParamater(object queryName, object queryValue, QueryType queryType,
            QueryRelatedType queryRelatedType = QueryRelatedType.AND)
        {
            IsInnerQuery = false;
            QueryName = queryName.ToString();
            QueryValue = queryValue.ToString();
            EQueryType = queryType;
            EQueryRelatedType = queryRelatedType;
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="queryName">查询字段名</param>
        /// <param name="queryValue">字段值</param>
        /// <param name="queryType">查询方式</param>
        /// <param name="isInnerQuery">是否是内部查询 </param>
        /// <param name="queryRelatedType">查询条件之间的关系类型枚举</param>
        public QueryParamater(object queryName, object queryValue, QueryType queryType,
            bool isInnerQuery,
            QueryRelatedType queryRelatedType = QueryRelatedType.AND)
        {
            IsInnerQuery = isInnerQuery;
            QueryName = queryName.ToString();
            QueryValue = queryValue.ToString();
            EQueryType = queryType;
            EQueryRelatedType = queryRelatedType;
        }

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="queryName">查询字段名</param>
        /// <param name="queryType">排序方式</param>
        /// <param name="queryRelatedType"> </param>
        public QueryParamater(object queryName, QueryType queryType,
            QueryRelatedType queryRelatedType = QueryRelatedType.AND)
        {
            QueryName = queryName.ToString();
            EQueryType = queryType;
            EQueryRelatedType = queryRelatedType;
        }

        public QueryParamater(QueryType queryType,
            QueryRelatedType queryRelatedType = QueryRelatedType.AND)
        {
            QueryName = string.Empty;
            EQueryType = queryType;
            EQueryRelatedType = queryRelatedType;
        }

        public QueryParamater(QueryType queryType, string queryValue)
        {
            QueryName = string.Empty;
            EQueryType = queryType;
            QueryValue = queryValue;
        }

        //组合排序使用
        public QueryParamater(Dictionary<object, QueryType> queryDictionary)
        {
            foreach (var queryType in queryDictionary)
            {
                QueryValue += queryType.Key.ToString();
                switch (queryType.Value)
                {
                    case QueryType.升序:
                        QueryValue += " ASC,";
                        break;
                    case QueryType.降序:
                        QueryValue += " DESC,";
                        break;
                }
            }
            EQueryType = QueryType.组合排序;
            if (QueryValue.Length > 1)
                QueryValue = QueryValue.Remove(QueryValue.Length - 1, 1);
        }

        /// <summary>
        ///     字段名
        /// </summary>
        public string QueryName { get; set; }

        /// <summary>
        ///     查询值
        /// </summary>
        public string QueryValue { get; set; }

        /// <summary>
        ///     查询条件枚举
        /// </summary>
        public QueryType EQueryType { get; set; }

        // 查询条件间的关系类型枚举  

        public QueryRelatedType EQueryRelatedType { get; set; }

        /// <summary>
        ///     是否是内部比对
        /// </summary>
        public bool IsInnerQuery { get; set; }

        //查询条件间的关系类型字符串 
        public string QueryRelatedTypeString => EQueryRelatedType == QueryRelatedType.EMPTY
            ? " "
            : $" {EQueryRelatedType} ";

        /// <summary>
        ///     将字段名、字段值、查询方式拼接成SQL查询字符串
        /// </summary>
        /// <returns>查询字符串</returns>
        public string toQueryString(string iIndex, out string returnStr)
        {
            returnStr = string.Empty;
            var QueryCondition = new StringBuilder();
            switch (EQueryType)
            {
                case QueryType.BETWEEN:
                {
                    var pars = QueryValue.Split(',');
                    if (pars.Length == 2)
                    {
                        QueryCondition.Append(QueryRelatedTypeString);
                        QueryCondition.Append(QueryName);
                        QueryCondition.Append("  >= @");
                        QueryCondition.Append(QueryName);
                        QueryCondition.Append(iIndex);
                        QueryCondition.Append("1");
                        QueryCondition.Append(QueryRelatedTypeString);
                        QueryCondition.Append(QueryName);
                        QueryCondition.Append("  <= @");
                        QueryCondition.Append(QueryName);
                        QueryCondition.Append(iIndex);
                        QueryCondition.Append("2");
                    }
                    break;
                }
                case QueryType.等于:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    if (IsInnerQuery)
                    {
                        QueryCondition.Append(" = ");
                        QueryCondition.Append(QueryValue);
                    }
                    else
                    {
                        QueryCondition.Append(" = @");
                        QueryCondition.Append(QueryName);
                        QueryCondition.Append(iIndex);
                    }
                    break;
                }
                case QueryType.不等于:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    if (IsInnerQuery)
                    {
                        QueryCondition.Append(" <> ");
                        QueryCondition.Append(QueryValue);
                    }
                    else
                    {
                        QueryCondition.Append("  <> @");
                        QueryCondition.Append(QueryName);
                        QueryCondition.Append(iIndex);
                    }
                    break;
                }
                case QueryType.大于:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    if (IsInnerQuery)
                    {
                        QueryCondition.Append(" > ");
                        QueryCondition.Append(QueryValue);
                    }
                    else
                    {
                        QueryCondition.Append("  > @");
                        QueryCondition.Append(QueryName);
                        QueryCondition.Append(iIndex);
                    }
                    break;
                }
                case QueryType.大于等于:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    if (IsInnerQuery)
                    {
                        QueryCondition.Append(" >= ");
                        QueryCondition.Append(QueryValue);
                    }
                    else
                    {
                        QueryCondition.Append("  >= @");
                        QueryCondition.Append(QueryName);
                        QueryCondition.Append(iIndex);
                    }
                    break;
                }
                case QueryType.小于:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    if (IsInnerQuery)
                    {
                        QueryCondition.Append(" < ");
                        QueryCondition.Append(QueryValue);
                    }
                    else
                    {
                        QueryCondition.Append(" < @");
                        QueryCondition.Append(QueryName);
                        QueryCondition.Append(iIndex);
                    }
                    break;
                }
                case QueryType.小于等于:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    if (IsInnerQuery)
                    {
                        QueryCondition.Append(" <= ");
                        QueryCondition.Append(QueryValue);
                    }
                    else
                    {
                        QueryCondition.Append(" <= @");
                        QueryCondition.Append(QueryName);
                        QueryCondition.Append(iIndex);
                    }
                    break;
                }
                case QueryType.LIKE:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(" LIKE @");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(iIndex);
                    break;
                }
                case QueryType.左LIKE:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(" LIKE @");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(iIndex);
                    break;
                }
                case QueryType.右LIKE:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(" LIKE @");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(iIndex);
                    break;
                }
                case QueryType.包含:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);

                    QueryCondition.AppendFormat(@" IN (SELECT * FROM @{0}table)",
                        QueryName + iIndex);

                    returnStr = string.Format(@"DECLARE @{0}table TABLE(SID VARCHAR(100));
                        INSERT INTO @{0}table (SID)
	                    SELECT WI.{0}.value('@i','varchar(100)') as Ids
	                    FROM @{0}.nodes('/es/e') WI({0});	", QueryName + iIndex);

                    break;
                }

                case QueryType.不包含:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);

                    QueryCondition.AppendFormat(@" NOT IN (SELECT * FROM @{0}table)",
                        QueryName + iIndex);

                    returnStr = string.Format(@"DECLARE @{0}table TABLE(SID VARCHAR(100));
                        INSERT INTO @{0}table (SID)
	                    SELECT WI.{0}.value('@i','varchar(100)') as Ids
	                    FROM @{0}.nodes('/es/e') WI({0});	", QueryName + iIndex);
                    break;
                }
                case QueryType.不为空:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(" IS NOT NULL ");
                    break;
                }
                case QueryType.为空:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(" IS NULL ");
                    break;
                }
                case QueryType.升序:
                {
                    QueryCondition.Append(" ORDER BY ");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(" ASC ");
                    break;
                }
                case QueryType.降序:
                {
                    QueryCondition.Append(" ORDER BY ");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(" DESC ");
                    break;
                }
                case QueryType.组合排序:
                {
                    QueryCondition.Append(" ORDER BY ");
                    QueryCondition.Append(QueryValue);
                    break;
                }
                case QueryType.左括号:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(" ( ");
                    break;
                }
                case QueryType.右括号:
                {
                    QueryCondition.Append(" ) ");
                    break;
                }
                case QueryType.或:
                {
                    QueryCondition.Append(" OR ");
                    break;
                }
                case QueryType.异或与:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(" ( ");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(" & @");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(iIndex);
                    QueryCondition.Append(" = @");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(iIndex);
                    QueryCondition.Append(" ) ");
                    break;
                }
                case QueryType.非异或与:
                {
                    QueryCondition.Append(QueryRelatedTypeString);
                    QueryCondition.Append(" ( ");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(" & @");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(iIndex);
                    QueryCondition.Append(" != @");
                    QueryCondition.Append(QueryName);
                    QueryCondition.Append(iIndex);
                    QueryCondition.Append(" ) ");
                    break;
                }
            }
            return QueryCondition.ToString();
        }
    }
}