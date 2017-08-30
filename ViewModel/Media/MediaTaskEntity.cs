using System;

namespace InstagramPhotos.Media.ViewModel
{
    /// <summary>
    /// MediaTaskEntity
    /// </summary>
    [Serializable]
    public class MediaTaskEntity
    {

        /// <summary>
        /// MediaTaskEntity 构造函数
        /// </summary>
        public MediaTaskEntity()
        { }


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


    }
}
