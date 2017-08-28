using InstagramPhotos.Utility.CommonQuery;
using System;
using System.Collections.Generic;

namespace InstagramPhotos.Media.QueryModel
{
    /// <summary>
    /// InsUserTypeQO
    /// </summary>
    public class InsUserTypeQO : IQueryEntity
    {

        /// <summary>
        /// InsUserTypeQO 构造函数
        /// </summary>
        public InsUserTypeQO()
        { }


        private string tablename = "Dim_InsUserType";
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
            get { return "InsUserTypeId"; }
        }

        public override List<QueryParamater> QueryPars { get; set; }

        #region 查询条件

        /// <summary>
        /// 通用查询条件
        /// </summary>
        public enum QueryEnums
        {
            /// <summary>
            /* 1: Instagram用户类型Id*/
            /// </summary>
            InsUserTypeId,
            /// <summary>
            /* 2: 用户类型名称*/
            /// </summary>
            TypeName,
            /// <summary>
            /* 3: 标签组*/
            /// </summary>
            TagGroupId,
            /// <summary>
            /* 4: 标签组(冗余字段)*/
            /// </summary>
            TagGroupName,
            /// <summary>
            /* 5: 排序值*/
            /// </summary>
            SortValue,
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
