using System;
using System.Collections.Generic;
using InstagramPhotos.Utility.CommonQuery;

namespace InstagramPhotos.Media.QueryModel
{
    /// <summary>
    /// MediaTaskQO
    /// </summary>
    public class MediaTaskQO : IQueryEntity
    {

        /// <summary>
        /// MediaTaskQO 构造函数
        /// </summary>
        public MediaTaskQO()
        { }


        private string tablename = "Fct_MediaTask";
        /// <summary>
        /// 表名
        /// </summary>
        public override string TableName
        {
            get { { return tablename; } }
            set { tablename = value; }
        }

        /// <summary>
        /// 查询主键名
        /// </summary>
        protected override string IdName
        {
            get { return "MediaTaskId"; }
        }

        public override List<QueryParamater> QueryPars { get; set; }

        #region 查询条件

        /// <summary>
        /// 通用查询条件
        /// </summary>
        public enum QueryEnums
        {
            /// <summary>
            /* 1: */
            /// </summary>
            MediaTaskId,
            /// <summary>
            /* 2: 网络资源路径*/
            /// </summary>
            Url,
            /// <summary>
            /* 3: 文件要存放的全路径*/
            /// </summary>
            FileFullName,
            /// <summary>
            /* 4: 指定要匹配的文件类型*/
            /// </summary>
            MetaTypeList,
            /// <summary>
            /* 5: 正则表达式*/
            /// </summary>
            RegexList,
            /// <summary>
            /* 6: 是否禁用 0正常 1禁用*/
            /// </summary>
            Disabled,
            /// <summary>
            /* 7: 创建人*/
            /// </summary>
            Rec_CreateBy,
            /// <summary>
            /* 8: 创建时间*/
            /// </summary>
            Rec_CreateTime,
            /// <summary>
            /* 9: 修改人*/
            /// </summary>
            Rec_ModifyBy,
            /// <summary>
            /* 10: 修改时间*/
            /// </summary>
            Rec_ModifyTime,
        }
        #endregion



    }
}
