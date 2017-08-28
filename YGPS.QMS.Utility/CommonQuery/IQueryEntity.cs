/*----------------------------------------
Developed by:wayne(chenwei@ccjoy-inc.com)
Date Created:2015-07-27
Last Updated:2015-07-27
Copyright:CCJoy
Description:通用查询实体基类
----------------------------------------*/

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using InstagramPhotos.Utility.Extension;

namespace InstagramPhotos.Utility.CommonQuery
{
    /// <summary>
    ///     通用查询实体
    /// </summary>
    public abstract class IQueryEntity
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        protected IQueryEntity()
        {
// ReSharper disable once DoNotCallOverridableMethodsInConstructor
            QueryPars = new List<QueryParamater>();
            AutoPriority = true;
        }

        /// <summary>
        ///     查询表名
        /// </summary>
        public abstract string TableName { get; set; }

        /// <summary>
        ///     查询ID
        /// </summary>
        protected abstract string IdName { get; }

        /// <summary>
        ///     查询合计列
        /// </summary>
        public string[] ColNames { get; set; }

        /// <summary>
        ///     是否查询所有列
        /// </summary>
        /// <summary>
        ///     是否分页
        /// </summary>
        public bool IsPage { get; set; }

        /// <summary>
        ///     页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     每页显示行数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     记录总行数
        /// </summary>
        public int TotalRowsCount { get; set; }

        /// <summary>
        ///     是否自动排序查询优先级别
        /// </summary>
        public bool AutoPriority { get; set; }

        /// <summary>
        ///     是否查询bool结果集  1.true 0.false
        /// </summary>
        public bool IsQueryExists { get; set; }

        public string OrderByString
        {
            get
            {
                var result = "";
                if (QueryPars.HasValidValues())
                {
                    QueryPars.ForEach(f =>
                    {
                        if (f.EQueryType == QueryParamater.QueryType.降序)
                            result += string.Format(" {0} desc,", f.QueryName);

                        if (f.EQueryType == QueryParamater.QueryType.升序)
                            result += string.Format(" {0} asc,", f.QueryName);

                        if (f.EQueryType == QueryParamater.QueryType.组合排序)
                            if (!string.IsNullOrEmpty(f.QueryValue))
                                result += f.QueryValue;
                    });
                }
                if (result.Length == 0)
                    result = IdName + " DESC";
                return result.TrimEnd(',');
            }
        }

        /// <summary>
        ///     查询参数（字段名、字段值、查询方式）
        /// </summary>
        public abstract List<QueryParamater> QueryPars { get; set; }

        /// <summary>
        ///     获取查询字符串（只读）
        /// </summary>
        /// <returns>查询字符串</returns>
        public string QueryString
        {
            get
            {
                var strbQueryCondition = new StringBuilder();

                strbQueryCondition.Append(Header);

                if (QueryPars.Count > 0)
                {
                    //参数优先级排序
                    if (AutoPriority)
                        QueryPars = QueryPars.OrderBy(c => (int) c.EQueryType).ToList();
                    var i = 0;
                    foreach (var c in QueryPars)
                    {
                        if (c.EQueryType == QueryParamater.QueryType.降序 || c.EQueryType == QueryParamater.QueryType.升序 ||
                            c.EQueryType == QueryParamater.QueryType.组合排序)
                        {
                            i++;
                            continue;
                        }

                        string returnStr;
                        strbQueryCondition.Append(c.toQueryString(i.ToString(CultureInfo.InvariantCulture),
                            out returnStr));

                        if (!string.IsNullOrEmpty(returnStr))
                            strbQueryCondition.Insert(0, returnStr);
                        i++;
                    }
                    bool HasFirst=false;
                    if (!IsPage &&
                        QueryPars.Count(
                            c =>
                                c.EQueryType == QueryParamater.QueryType.降序 ||
                                c.EQueryType == QueryParamater.QueryType.升序 ||
                                c.EQueryType == QueryParamater.QueryType.组合排序) > 0)
                    {
                        foreach (
                            var c in
                                QueryPars.Where(
                                    c =>
                                        c.EQueryType == QueryParamater.QueryType.降序 ||
                                        c.EQueryType == QueryParamater.QueryType.升序 ||
                                        c.EQueryType == QueryParamater.QueryType.组合排序))
                        {
                            string returnStr;
                            var value =c.toQueryString(i.ToString(CultureInfo.InvariantCulture), out returnStr);
                            if (HasFirst)
                                value=returnStr.Replace(" ORDER BY ", ",");
                            else
                            {
                                HasFirst = true;
                            }
                            strbQueryCondition.Append(value);
                        }
                    }
                }

                strbQueryCondition.Append(Footer);

                return strbQueryCondition.ToString();
            }
        }

        public string Header
        {
            get
            {
                if (IsPage)
                {
                    return string.Format(
                        @" WITH T AS( SELECT {0}, ROW_NUMBER() OVER(ORDER BY {2}) R FROM {1} WITH(NOLOCK) WHERE 1=1 ",
                        IdName
                        //IsQueryAllColumn ? "*" : IdName
                        , TableName, OrderByString);
                }
                return string.Format("SELECT {0} FROM {1} WITH(NOLOCK) WHERE 1=1", IdName
                    //IsQueryAllColumn ? "*" : IdName
                    , TableName);
            }
        }

        public string Footer
        {
            get
            {
                if (IsPage)
                    return
                        string.Format(
                            @" )SELECT {0},(SELECT COUNT(1) FROM T) AS TotalDataCount FROM T WITH(NOLOCK) WHERE R>{1} AND R<={2}",
                            IdName
                            //IsQueryAllColumn ? "*" : IdName
                            , (PageIndex - 1)*PageSize, PageIndex*PageSize);
                return "";
            }
        }

        #region 查询方法

        public IQueryEntity Equal(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.等于, queryRelatedType));
            return this;
        }

        public IQueryEntity NotEqual(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.不等于, queryRelatedType));
            return this;
        }

        public IQueryEntity Like(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.LIKE, queryRelatedType));
            return this;
        }

        public IQueryEntity RightLike(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.右LIKE, queryRelatedType));
            return this;
        }

        public IQueryEntity LeftLike(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.左LIKE, queryRelatedType));
            return this;
        }

        public IQueryEntity MoreThan(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.大于, queryRelatedType));
            return this;
        }

        public IQueryEntity LessThan(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.小于, queryRelatedType));
            return this;
        }

        public IQueryEntity NotMoreThan(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.小于等于, queryRelatedType));
            return this;
        }

        public IQueryEntity NotLessThan(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.大于等于, queryRelatedType));
            return this;
        }

        public IQueryEntity Contains(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.包含, queryRelatedType));
            return this;
        }

        public IQueryEntity NotContains(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.不包含, queryRelatedType));
            return this;
        }

        public IQueryEntity XOR(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.异或与, queryRelatedType));
            return this;
        }

        public IQueryEntity NotXOR(object column, object value,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, value, QueryParamater.QueryType.非异或与, queryRelatedType));
            return this;
        }

        public IQueryEntity IsEmpty(object column,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, QueryParamater.QueryType.为空, queryRelatedType));
            return this;
        }

        public IQueryEntity NotEmpty(object column,
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(column, QueryParamater.QueryType.不为空, queryRelatedType));
            return this;
        }

        public IQueryEntity LeftBracket(
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.AND)
        {
            QueryPars.Add(new QueryParamater(QueryParamater.QueryType.左括号, queryRelatedType));
            return this;
        }

        public IQueryEntity RightBracket(
            QueryParamater.QueryRelatedType queryRelatedType = QueryParamater.QueryRelatedType.EMPTY)
        {
            QueryPars.Add(new QueryParamater(QueryParamater.QueryType.右括号, queryRelatedType));
            return this;
        }


        /// <summary>
        /// </summary>
        /// <param name="columnName"></param>
        public IQueryEntity OrderByAsc(object columnName)
        {
            QueryPars.Add(new QueryParamater(columnName, QueryParamater.QueryType.升序));
            return this;
        }

        /// <summary>
        /// </summary>
        /// <param name="columnName"></param>
        public IQueryEntity OrderByDesc(object columnName)
        {
            QueryPars.Add(new QueryParamater(columnName, QueryParamater.QueryType.降序));
            return this;
        }
        #endregion
    }
}