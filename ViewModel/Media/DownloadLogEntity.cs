using System;

namespace InstagramPhotos.ViewModel
{
    /// <summary>
    /// DownloadLogEntity
    /// </summary>
    [Serializable]
    public class DownloadLogEntity
    {

        /// <summary>
        /// DownloadLogEntity 构造函数
        /// </summary>
        public DownloadLogEntity()
        { }


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


    }
}
