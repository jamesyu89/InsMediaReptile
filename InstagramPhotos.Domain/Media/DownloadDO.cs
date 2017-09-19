﻿using System;

namespace InstagramPhotos.DomainModel
{
    /// <summary>
    /// DownloadDO
    /// </summary>
    [Serializable]
    public class DownloadDO
    {

        /// <summary>
        /// DownloadDO 构造函数
        /// </summary>
        public DownloadDO()
        { }


        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName
        {
            get { return "Fct_Download"; }
        }

        /// <summary>
        /// 查询主键名
        /// </summary>
        public static string IdName
        {
            get { return "DownloadId"; }
        }

        #region Members


        #endregion

        /// <summary>
        /* 1: */
        /// </summary>
        public Guid DownloadId { get; set; }
        /// <summary>
        /* 2: */
        /// </summary>
        public string HttpUrl { get; set; }
        /// <summary>
        /* 3: 排序值*/
        /// </summary>
        public string SortValue { get; set; }
        /// <summary>
        /* 4: 是否禁用 0正常 1禁用*/
        /// </summary>
        public int? Disabled { get; set; }
        /// <summary>
        /* 5: 创建人*/
        /// </summary>
        public Guid? Rec_CreateBy { get; set; }
        /// <summary>
        /* 6: 创建时间*/
        /// </summary>
        public DateTime? Rec_CreateTime { get; set; }
        /// <summary>
        /* 7: 修改人*/
        /// </summary>
        public Guid? Rec_ModifyBy { get; set; }
        /// <summary>
        /* 8: 修改时间*/
        /// </summary>
        public DateTime? Rec_ModifyTime { get; set; }
        #region 列名

        /// <summary>
        /// 列名枚举
        /// </summary>
        public enum ColumnEnum
        {
            /// <summary>
            /* 9: */
            /// </summary>
            DownloadId,
            /// <summary>
            /* 10: */
            /// </summary>
            HttpUrl,
            /// <summary>
            /* 11: 排序值*/
            /// </summary>
            SortValue,
            /// <summary>
            /* 12: 是否禁用 0正常 1禁用*/
            /// </summary>
            Disabled,
            /// <summary>
            /* 13: 创建人*/
            /// </summary>
            Rec_CreateBy,
            /// <summary>
            /* 14: 创建时间*/
            /// </summary>
            Rec_CreateTime,
            /// <summary>
            /* 15: 修改人*/
            /// </summary>
            Rec_ModifyBy,
            /// <summary>
            /* 16: 修改时间*/
            /// </summary>
            Rec_ModifyTime,
        }
        #endregion



    }
}
