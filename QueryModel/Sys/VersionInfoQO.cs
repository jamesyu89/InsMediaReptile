using System;
using System.Collections.Generic;
using InstagramPhotos.Utility.CommonQuery;

namespace InstagramPhotos.QueryModel.Sys
{
    /// <summary>
    /// VersionInfoQO
    /// </summary>
    public class VersionInfoQO : IQueryEntity
    {

        /// <summary>
        /// VersionInfoQO 构造函数
        /// </summary>
        public VersionInfoQO()
        { }


        private string tablename = "Dim_Sys_VersionInfo";
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
            get { return "VersionId"; }
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
            VersionId,
            /// <summary>
            /* 2: 版本类型 1 Pad*/
            /// </summary>
            VersionType,
            /// <summary>
            /* 3: 版本号 (1.0.0)*/
            /// </summary>
            Version,
            /// <summary>
            /* 4: 升级类型 0 强制升级 1 可升级 2 无更新*/
            /// </summary>
            UpdateType,
            /// <summary>
            /* 5: 升级信息*/
            /// </summary>
            UpdateInfo,
            /// <summary>
            /* 6: 下载地址*/
            /// </summary>
            UpdateUrl,
            /// <summary>
            /* 7: 是否禁用 0 非禁用 1 禁用 */
            /// </summary>
            Disabled,
            /// <summary>
            /* 8: 创建人*/
            /// </summary>
            Rec_CreateBy,
            /// <summary>
            /* 9: 创建时间*/
            /// </summary>
            Rec_CreateTime,
            /// <summary>
            /* 10: 修改时间*/
            /// </summary>
            Rec_ModifyTime,
            /// <summary>
            /* 11: 修改人名称*/
            /// </summary>
            Rec_ModifyBy
        }
        #endregion



    }
}
