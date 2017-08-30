using InstagramPhotos.Media.DomainModel;
using InstagramPhotos.Media.QueryModel;
using InstagramPhotos.Media.ViewModel;
using InstagramPhotos.Repository;
using InstagramPhotos.Utility.CommonQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using Service.BLL.Media.Mapping;
using Service.Interface.Media;

namespace Service.BLL.Media
{
    public class MediaService : IMediaService
    {
        #region Auto Service

        #region auto Media

        /// <summary>
        ///     新增Media信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        public void AddMedia(MediaEntity dto)
        {
            MediaDO info = dto.ConvertToModel();
            new MediaRepository().AddMedia(info);
        }

        /// <summary>
        ///     更新Media信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        public void UpdateMedia(MediaEntity dto)
        {
            MediaDO info = dto.ConvertToModel();
            new MediaRepository().UpdateMedia(info);
            MediaCommon.cache_Media.Remove(info.MediaID);
        }

        /// <summary>
        ///    获取Media信息
        /// </summary>
        /// <param name="MediaID">主键</param>
        public MediaEntity GetMedia(Guid MediaID)
        {
            MediaDO dto = MediaCommon.cache_Media.GetFromDB(MediaID, new MediaRepository().GetMedia);
            return dto.ConvertToDto();
        }

        /// <summary>
        ///    根据MediaIDs数组获取Media信息列表
        /// </summary>
        /// <param name="MediaIDs">主键集合</param>
        /// <returns>Media信息列表</returns>
        public List<MediaEntity> GetMedias(Guid[] MediaIDs)
        {
            return MediaCommon.cache_Media.GetFromDB(MediaIDs, new MediaRepository().GetMedias)
                    .Select(m => m.ConvertToDto())
                    .ToList();
        }

        /// <summary>
        ///  通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="isCache"></param>
        /// <returns>MediaEntity信息列表</returns>
        public List<MediaEntity> GetMediaDtosByPara(MediaQO queryEntity, Boolean isCache)
        {
            var qm = new QueryHelper(new MediaRepository());
            List<Guid> ids = qm.GetGuidIDsByConditions(queryEntity, isCache);
            if (ids == null || ids.Count == 0)
                return new List<MediaEntity>();
            return GetMedias(ids.ToArray());
        }

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
        public List<MediaEntity> GetMediaDtosByParaForPage(MediaQO queryEntity,
            Int32 pageIndex, Int32 pageSize, out Int32 totalCount, out Int32 pageCount, Boolean isCache)
        {
            var qm = new QueryHelper(new MediaRepository());
            List<Guid> ids = qm.GetGuidIDsByConditions(queryEntity, pageIndex, pageSize, out totalCount, out pageCount, isCache);
            if (ids == null || ids.Count == 0)
                return new List<MediaEntity>();
            return GetMedias(ids.ToArray());
        }

        #endregion

        #region auto MediaTask

        /// <summary>
        ///     新增MediaTask信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        public void AddMediatask(MediaTaskEntity dto)
        {
            MediaTaskDO info = dto.ConvertToModel();
            new MediaRepository().AddMediatask(info);
        }

        /// <summary>
        ///     更新MediaTask信息
        /// </summary>
        /// <param name="dto">ViewModel</param>
        public void UpdateMediatask(MediaTaskEntity dto)
        {
            MediaTaskDO info = dto.ConvertToModel();
            new MediaRepository().UpdateMediatask(info);
            MediaCommon.cache_MediaTask.Remove(info.MediaTaskId);
        }

        /// <summary>
        ///    获取MediaTask信息
        /// </summary>
        /// <param name="MediaTaskId">主键</param>
        public MediaTaskEntity GetMediatask(Guid MediaTaskId)
        {
            MediaTaskDO dto = MediaCommon.cache_MediaTask.GetFromDB(MediaTaskId, new MediaRepository().GetMediatask);
            return dto.ConvertToDto();
        }

        /// <summary>
        ///    根据MediaTaskIds数组获取MediaTask信息列表
        /// </summary>
        /// <param name="MediaTaskIds">主键集合</param>
        /// <returns>MediaTask信息列表</returns>
        public List<MediaTaskEntity> GetMediatasks(Guid[] MediaTaskIds)
        {
            return
                MediaCommon.cache_MediaTask.GetFromDB(MediaTaskIds, new MediaRepository().GetMediatasks)
                    .Select(m => m.ConvertToDto())
                    .ToList();
        }

        /// <summary>
        ///  通用查询
        /// </summary>
        /// <param name="queryEntity"></param>
        /// <param name="isCache"></param>
        /// <returns>MediaTaskEntity信息列表</returns>
        public List<MediaTaskEntity> GetMediataskDtosByPara(MediaTaskQO queryEntity, Boolean isCache)
        {
            var qm = new QueryHelper(new MediaRepository());
            List<Guid> ids = qm.GetGuidIDsByConditions(queryEntity, isCache);
            if (ids == null || ids.Count == 0)
                return new List<MediaTaskEntity>();
            return GetMediatasks(ids.ToArray());
        }

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
        public List<MediaTaskEntity> GetMediataskDtosByParaForPage(MediaTaskQO queryEntity,
            Int32 pageIndex, Int32 pageSize, out Int32 totalCount, out Int32 pageCount, Boolean isCache)
        {
            var qm = new QueryHelper(new MediaRepository());
            List<Guid> ids = qm.GetGuidIDsByConditions(queryEntity, pageIndex, pageSize, out totalCount, out pageCount, isCache);
            if (ids == null || ids.Count == 0)
                return new List<MediaTaskEntity>();
            return GetMediatasks(ids.ToArray());
        }
        /// <summary>
        /// 批量添加媒体任务
        /// </summary>
        /// <param name="dto"></param>
        public void BatchAddMediatask(List<MediaTaskEntity> dto)
        {
            var repository = new MediaRepository();
            var list = new List<MediaTaskDO>();
            dto.ForEach(d => list.Add(d.ConvertToModel()));
            new MediaRepository().AddMediataskList(list);
        }

        #endregion

        #endregion
    }
}
