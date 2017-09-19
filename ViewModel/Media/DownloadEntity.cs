using System;

namespace InstagramPhotos.ViewModel
{
    /// <summary>
    /// DownloadEntity
    /// </summary>
    [Serializable]
    public class DownloadEntity
    {

        /// <summary>
        /// DownloadEntity 构造函数
        /// </summary>
        public DownloadEntity()
        { }


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


    }
}
