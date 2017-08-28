using InstagramPhotos.Utility.CommonQuery;
using System;
using System.Collections.Generic;

namespace InstagramPhotos.Media.QueryModel
{
    /// <summary>
    /// MediaQO
    /// </summary>
    public class MediaQO : IQueryEntity
    {

        /// <summary>
        /// MediaQO 构造函数
        /// </summary>
        public MediaQO()
        { }


        private string tablename = "Fct_Media";
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
            get { return "MediaID"; }
        }

        public override List<QueryParamater> QueryPars { get; set; }

        #region 查询条件

        /// <summary>
        /// 通用查询条件
        /// </summary>
        public enum QueryEnums
        {
            /// <summary>
            /* 1: 媒体Id*/
            /// </summary>
            MediaID,
            /// <summary>
            /* 2: 媒体编号*/
            /// </summary>
            MediaCode,
            /// <summary>
            /* 3: 媒体名称*/
            /// </summary>
            MediaName,
            /// <summary>
            /* 4: 标签Id*/
            /// </summary>
            TagId,
            /// <summary>
            /* 5: 媒体资源来自的Ins用户*/
            /// </summary>
            FromInsUser,
            /// <summary>
            /* 6: 资源完整地址*/
            /// </summary>
            Url,
            /// <summary>
            /* 7: 资源相对站点根目录的地址*/
            /// </summary>
            RelativeAddress,
            /// <summary>
            /* 8: 资源存放的物理地址*/
            /// </summary>
            PhycialAddress,
            /// <summary>
            /* 9: 资源大小(KB)*/
            /// </summary>
            Size,
            /// <summary>
            /* 10: 资源下载开始时间*/
            /// </summary>
            Download_Start,
            /// <summary>
            /* 11: 资源下载结束时间*/
            /// </summary>
            Download_End,
            /// <summary>
            /* 12: 下载是否成功 0失败 1成功*/
            /// </summary>
            Download_Ok,
            /// <summary>
            /* 13: */
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
