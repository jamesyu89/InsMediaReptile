using System;

namespace InstagramPhotos.Media.DomainModel
{
    /// <summary>
    /// InsUserTypeDO
    /// </summary>
    [Serializable]
    public class InsUserTypeDO
    {

        /// <summary>
        /// InsUserTypeDO 构造函数
        /// </summary>
        public InsUserTypeDO()
        { }


        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName
        {
            get { return "Dim_InsUserType"; }
        }

        /// <summary>
        /// 查询主键名
        /// </summary>
        public static string IdName
        {
            get { return "InsUserTypeId"; }
        }

        #region Members


        #endregion

        /// <summary>
        /* 1: Instagram用户类型Id*/
        /// </summary>
        public Guid InsUserTypeId { get; set; }
        /// <summary>
        /* 2: 用户类型名称*/
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /* 3: 标签组*/
        /// </summary>
        public Guid? TagGroupId { get; set; }
        /// <summary>
        /* 4: 标签组(冗余字段)*/
        /// </summary>
        public string TagGroupName { get; set; }
        /// <summary>
        /* 5: 排序值*/
        /// </summary>
        public string SortValue { get; set; }
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
            /* 11: Instagram用户类型Id*/
            /// </summary>
            InsUserTypeId,
            /// <summary>
            /* 12: 用户类型名称*/
            /// </summary>
            TypeName,
            /// <summary>
            /* 13: 标签组*/
            /// </summary>
            TagGroupId,
            /// <summary>
            /* 14: 标签组(冗余字段)*/
            /// </summary>
            TagGroupName,
            /// <summary>
            /* 15: 排序值*/
            /// </summary>
            SortValue,
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
