using System;

namespace InstagramPhotos.DomainModel
{
    /// <summary>
    /// DownloadLogDO
    /// </summary>
    [Serializable]
    public class DownloadLogDO
    {

        /// <summary>
        /// DownloadLogDO 构造函数
        /// </summary>
        public DownloadLogDO()
        { }


        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName
        {
            get { return "Rel_DownloadLog"; }
        }

        /// <summary>
        /// 查询主键名
        /// </summary>
        public static string IdName
        {
            get { return "LogId"; }
        }

        #region Members


        #endregion

        /// <summary>
        /* 1: */
        /// </summary>
        public Guid LogId { get; set; }
        /// <summary>
        /* 2: */
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /* 3: */
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /* 4: 排序值*/
        /// </summary>
        public string SortValue { get; set; }
        /// <summary>
        /* 5: 是否禁用 0正常 1禁用*/
        /// </summary>
        public int? Disabled { get; set; }
        /// <summary>
        /* 6: 创建人*/
        /// </summary>
        public Guid? Rec_CreateBy { get; set; }
        /// <summary>
        /* 7: 创建时间*/
        /// </summary>
        public DateTime? Rec_CreateTime { get; set; }
        /// <summary>
        /* 8: 修改人*/
        /// </summary>
        public Guid? Rec_ModifyBy { get; set; }
        /// <summary>
        /* 9: 修改时间*/
        /// </summary>
        public DateTime? Rec_ModifyTime { get; set; }
        #region 列名

        /// <summary>
        /// 列名枚举
        /// </summary>
        public enum ColumnEnum
        {
            /// <summary>
            /* 10: */
            /// </summary>
            LogId,
            /// <summary>
            /* 11: */
            /// </summary>
            Message,
            /// <summary>
            /* 12: */
            /// </summary>
            Level,
            /// <summary>
            /* 13: 排序值*/
            /// </summary>
            SortValue,
            /// <summary>
            /* 14: 是否禁用 0正常 1禁用*/
            /// </summary>
            Disabled,
            /// <summary>
            /* 15: 创建人*/
            /// </summary>
            Rec_CreateBy,
            /// <summary>
            /* 16: 创建时间*/
            /// </summary>
            Rec_CreateTime,
            /// <summary>
            /* 17: 修改人*/
            /// </summary>
            Rec_ModifyBy,
            /// <summary>
            /* 18: 修改时间*/
            /// </summary>
            Rec_ModifyTime,
        }
        #endregion



    }
}
