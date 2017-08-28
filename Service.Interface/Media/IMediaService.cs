using InstagramPhotos.Media.QueryModel;
using InstagramPhotos.Media.ViewModel;
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

    }
}
