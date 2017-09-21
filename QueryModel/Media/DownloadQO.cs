using System;
using System.Collections.Generic;
using InstagramPhotos.Utility.CommonQuery;

namespace InstagramPhotos.QueryModel
{
    /// <summary>
    /// DownloadQO
    /// </summary>
    public class DownloadQO : IQueryEntity
    {

        /// <summary>
        /// DownloadQO 构造函数
        /// </summary>
        public DownloadQO()
        { }


        private string tablename = "Fct_Download";
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
            get { return "DownloadId"; }
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
            DownloadId,
            /// <summary>
            /* 2: */
            /// </summary>
            HttpUrl,
            /// <summary>
            /* 3: */
            /// </summary>
            DirName,
            /// <summary>
            /* 4: 排序值*/
            /// </summary>
            SortValue,
            /// <summary>
            /* 5: 是否禁用 0正常 1禁用*/
            /// </summary>
            Disabled,
            /// <summary>
            /* 6: 创建人*/
            /// </summary>
            Rec_CreateBy,
            /// <summary>
            /* 7: 创建时间*/
            /// </summary>
            Rec_CreateTime,
            /// <summary>
            /* 8: 修改人*/
            /// </summary>
            Rec_ModifyBy,
            /// <summary>
            /* 9: 修改时间*/
            /// </summary>
            Rec_ModifyTime,
        }
        #endregion



    }
}
