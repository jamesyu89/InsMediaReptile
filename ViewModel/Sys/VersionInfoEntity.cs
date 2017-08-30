using System;

namespace InstagramPhotos.ViewModel.Sys
{
    /// <summary>
    /// VersionInfoEntity
    /// </summary>
    [Serializable]
    public class VersionInfoEntity
    {

        /// <summary>
        /// VersionInfoEntity 构造函数
        /// </summary>
        public VersionInfoEntity()
        { }


        #region Members


        #endregion

        /// <summary>
        /* 1: */
        /// </summary>
        public Guid VersionId { get; set; }
        /// <summary>
        /* 2: 版本类型 1 Pad*/
        /// </summary>
        public int VersionType { get; set; }
        /// <summary>
        /* 3: 版本号 (1.0.0)*/
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /* 4: 升级类型 0 强制升级 1 可升级 2 无更新*/
        /// </summary>
        public int UpdateType { get; set; }
        /// <summary>
        /* 5: 升级信息*/
        /// </summary>
        public string UpdateInfo { get; set; }
        /// <summary>
        /* 6: 下载地址*/
        /// </summary>
        public string UpdateUrl { get; set; }
        /// <summary>
        /* 7: 是否禁用 0 非禁用 1 禁用 */
        /// </summary>
        public bool Disabled { get; set; }
        /// <summary>
        /* 8: 创建人*/
        /// </summary>
        public string Rec_CreateBy { get; set; }
        /// <summary>
        /* 9: 创建时间*/
        /// </summary>
        public DateTime Rec_CreateTime { get; set; }
        /// <summary>
        /* 10: 修改时间*/
        /// </summary>
        public DateTime? Rec_ModifyTime { get; set; }
        /// <summary>
        /* 11: 修改人名称*/
        /// </summary>
        public string Rec_ModifyBy { get; set; }


    }
}
