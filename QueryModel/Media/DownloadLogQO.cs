using System;
using System.Collections.Generic;
using InstagramPhotos.Utility.CommonQuery;

namespace InstagramPhotos.QueryModel
{
    /// <summary>
    /// DownloadLogQO
    /// </summary>
    public class DownloadLogQO : IQueryEntity
    {

        /// <summary>
        /// DownloadLogQO 构造函数
        /// </summary>
        public DownloadLogQO()
        { }


        private string tablename = "Rel_DownloadLog";
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
            get { return "LogId"; }
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
            LogId,
            /// <summary>
            /* 2: */
            /// </summary>
            Message,
            /// <summary>
            /* 3: */
            /// </summary>
            Level,
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
