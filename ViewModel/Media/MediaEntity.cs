using System;

namespace InstagramPhotos.Media.ViewModel
{
    /// <summary>
    /// MediaEntity
    /// </summary>
    [Serializable]
    public class MediaEntity
    {

        /// <summary>
        /// MediaEntity 构造函数
        /// </summary>
        public MediaEntity()
        { }


        #region Members


        #endregion

        /// <summary>
        /* 1: 媒体Id*/
        /// </summary>
        public Guid MediaID { get; set; }
        /// <summary>
        /* 2: 媒体编号*/
        /// </summary>
        public string MediaCode { get; set; }
        /// <summary>
        /* 3: 媒体名称*/
        /// </summary>
        public string MediaName { get; set; }
        /// <summary>
        /* 4: 标签Id*/
        /// </summary>
        public Guid? TagId { get; set; }
        /// <summary>
        /* 5: 媒体资源来自的Ins用户*/
        /// </summary>
        public Guid? FromInsUser { get; set; }
        /// <summary>
        /* 6: 资源完整地址*/
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /* 7: 资源相对站点根目录的地址*/
        /// </summary>
        public string RelativeAddress { get; set; }
        /// <summary>
        /* 8: 资源存放的物理地址*/
        /// </summary>
        public string PhycialAddress { get; set; }
        /// <summary>
        /* 9: 资源大小(KB)*/
        /// </summary>
        public Int64? Size { get; set; }
        /// <summary>
        /* 10: 资源下载开始时间*/
        /// </summary>
        public DateTime? Download_Start { get; set; }
        /// <summary>
        /* 11: 资源下载结束时间*/
        /// </summary>
        public DateTime? Download_End { get; set; }
        /// <summary>
        /* 12: 下载是否成功 0失败 1成功*/
        /// </summary>
        public int? Download_Ok { get; set; }
        /// <summary>
        /* 13: */
        /// </summary>
        public string SortValue { get; set; }
        /// <summary>
        /* 14: 是否禁用 0正常 1禁用*/
        /// </summary>
        public int? Disabled { get; set; }
        /// <summary>
        /* 15: 创建人*/
        /// </summary>
        public Guid? Rec_CreateBy { get; set; }
        /// <summary>
        /* 16: 创建时间*/
        /// </summary>
        public DateTime? Rec_CreateTime { get; set; }
        /// <summary>
        /* 17: 修改人*/
        /// </summary>
        public Guid? Rec_ModifyBy { get; set; }
        /// <summary>
        /* 18: 修改时间*/
        /// </summary>
        public DateTime? Rec_ModifyTime { get; set; }


    }
}
