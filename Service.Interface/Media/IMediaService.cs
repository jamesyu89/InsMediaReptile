using InstagramPhotos.Media.QueryModel;
using InstagramPhotos.Media.ViewModel;
using InstagramPhotos.QueryModel;
using InstagramPhotos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface.Media
{
    public interface IMediaService
    {
        #region auto Media

        /// <summary>
        ///     新增Media信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        void AddMedia(MediaEntity dto);

        /// <summary>
        ///     更新Media信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        void UpdateMedia(MediaEntity dto);

        /// <summary>
        ///    获取MediaEntity信息
        /// </summary>
        /// <param name="MediaID">主键</param>
        MediaEntity GetMedia(Guid MediaID);

        /// <summary>
        ///    根据MediaIDs数组获取Media信息列表
        /// </summary>
        /// <param name="MediaIDs">主键集合</param>
        /// <returns>MediaEntity信息列表</returns>
        List<MediaEntity> GetMedias(Guid[] MediaIDs);

        /// <summary>
        ///  通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="isCache"></param>
        /// <returns>MediaEntity信息列表</returns>
        List<MediaEntity> GetMediaDtosByPara(MediaQO queryEntity, Boolean isCache);

        /// <summary>
        ///  分页通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="isCache"></param>
        /// <returns>MediaEntity信息列表</returns>
        List<MediaEntity> GetMediaDtosByParaForPage(MediaQO queryEntity,
            Int32 pageIndex, Int32 pageSize, out Int32 totalCount, out Int32 pageCount, Boolean isCache);

        #endregion

        #region auto MediaTask

        /// <summary>
        ///     新增MediaTask信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        void AddMediatask(MediaTaskEntity dto);

        void BatchAddMediatask(List<MediaTaskEntity> dto);

        /// <summary>
        ///     更新MediaTask信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        void UpdateMediatask(MediaTaskEntity dto);

        /// <summary>
        ///    获取MediaTaskEntity信息
        /// </summary>
        /// <param name="MediaTaskId">主键</param>
        MediaTaskEntity GetMediatask(Guid MediaTaskId);

        /// <summary>
        ///    根据MediaTaskIds数组获取MediaTask信息列表
        /// </summary>
        /// <param name="MediaTaskIds">主键集合</param>
        /// <returns>MediaTaskEntity信息列表</returns>
        List<MediaTaskEntity> GetMediatasks(Guid[] MediaTaskIds);

        /// <summary>
        ///  通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="isCache"></param>
        /// <returns>MediaTaskEntity信息列表</returns>
        List<MediaTaskEntity> GetMediataskDtosByPara(MediaTaskQO queryEntity, Boolean isCache);

        /// <summary>
        ///  分页通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="isCache"></param>
        /// <returns>MediaTaskEntity信息列表</returns>
        List<MediaTaskEntity> GetMediataskDtosByParaForPage(MediaTaskQO queryEntity,
            Int32 pageIndex, Int32 pageSize, out Int32 totalCount, out Int32 pageCount, Boolean isCache);

        #endregion

        #region auto Download

        /// <summary>
        ///     新增Download信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        void AddDownload(DownloadEntity dto);

        /// <summary>
        ///     更新Download信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        void UpdateDownload(DownloadEntity dto);

        /// <summary>
        ///    获取DownloadEntity信息
        /// </summary>
        /// <param name="DownloadId">主键</param>
        DownloadEntity GetDownload(Guid DownloadId);

        /// <summary>
        ///    根据DownloadIds数组获取Download信息列表
        /// </summary>
        /// <param name="DownloadIds">主键集合</param>
        /// <returns>DownloadEntity信息列表</returns>
        List<DownloadEntity> GetDownloads(Guid[] DownloadIds,bool iscache);

        /// <summary>
        ///  通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="isCache"></param>
        /// <returns>DownloadEntity信息列表</returns>
        List<DownloadEntity> GetDownloadDtosByPara(DownloadQO queryEntity, Boolean isCache);

        /// <summary>
        ///  分页通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageCount"></param>
        /// <param name="isCache"></param>
        /// <returns>DownloadEntity信息列表</returns>
        List<DownloadEntity> GetDownloadDtosByParaForPage(DownloadQO queryEntity,
            Int32 pageIndex, Int32 pageSize, out Int32 totalCount, out Int32 pageCount, Boolean isCache);

        #endregion

    }
}
