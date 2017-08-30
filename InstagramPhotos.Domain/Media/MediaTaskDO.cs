using System;

namespace InstagramPhotos.Media.DomainModel
{
    /// <summary>
    /// MediaTaskDO
    /// </summary>
    [Serializable]
    public class MediaTaskDO
    {

        /// <summary>
        /// MediaTaskDO 构造函数
        /// </summary>
        public MediaTaskDO()
        { }


        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName
        {
            get { return "Fct_MediaTask"; }
        }

        /// <summary>
        /// 查询主键名
        /// </summary>
        public static string IdName
        {
            get { return "MediaTaskId"; }
        }

        #region Members


        #endregion

        /// <summary>
        /* 1: */
        /// </summary>
        public Guid MediaTaskId { get; set; }
        /// <summary>
        /* 2: 网络资源路径*/
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /* 3: 文件要存放的全路径*/
        /// </summary>
        public string FileFullName { get; set; }
        /// <summary>
        /* 4: 指定要匹配的文件类型*/
        /// </summary>
        public string MetaTypeList { get; set; }
        /// <summary>
        /* 5: 正则表达式*/
        /// </summary>
        public string RegexList { get; set; }
        /// <summary>
        /* 6: 是否禁用 0正常 1禁用*/
        /// </summary>
        public int? Disabled { get; set; }
        /// <summary>
        /* 7: 创建人*/
        /// </summary>
        public Guid? Rec_CreateBy { get; set; }
        /// <summary>
        /* 8: 创建时间*/
        /// </summary>
        public DateTime? Rec_CreateTime { get; set; }
        /// <summary>
        /* 9: 修改人*/
        /// </summary>
        public Guid? Rec_ModifyBy { get; set; }
        /// <summary>
        /* 10: 修改时间*/
        /// </summary>
        public DateTime? Rec_ModifyTime { get; set; }
        #region 列名

        /// <summary>
        /// 列名枚举
        /// </summary>
        public enum ColumnEnum
        {
            /// <summary>
            /* 11: */
            /// </summary>
            MediaTaskId,
            /// <summary>
            /* 12: 网络资源路径*/
            /// </summary>
            Url,
            /// <summary>
            /* 13: 文件要存放的全路径*/
            /// </summary>
            FileFullName,
            /// <summary>
            /* 14: 指定要匹配的文件类型*/
            /// </summary>
            MetaTypeList,
            /// <summary>
            /* 15: 正则表达式*/
            /// </summary>
            RegexList,
            /// <summary>
            /* 16: 是否禁用 0正常 1禁用*/
            /// </summary>
            Disabled,
            /// <summary>
            /* 17: 创建人*/
            /// </summary>
            Rec_CreateBy,
            /// <summary>
            /* 18: 创建时间*/
            /// </summary>
            Rec_CreateTime,
            /// <summary>
            /* 19: 修改人*/
            /// </summary>
            Rec_ModifyBy,
            /// <summary>
            /* 20: 修改时间*/
            /// </summary>
            Rec_ModifyTime,
        }
        #endregion



    }
}
