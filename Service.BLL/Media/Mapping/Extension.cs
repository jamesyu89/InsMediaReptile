using EmitMapper;
using InstagramPhotos.DomainModel;
using InstagramPhotos.Media.DomainModel;
using InstagramPhotos.Media.ViewModel;
using InstagramPhotos.ViewModel;

namespace Service.BLL.Media.Mapping
{
    internal static class Extension
    {
        #region Media

        private static readonly ObjectsMapper<MediaEntity, MediaDO> mapper_MediaEntity_2_MediaDO = ObjectMapperManager.DefaultInstance.GetMapper<MediaEntity, MediaDO>();

        private static readonly ObjectsMapper<MediaDO, MediaEntity> mapper_MediaDO_2_MediaEntity = ObjectMapperManager.DefaultInstance.GetMapper<MediaDO, MediaEntity>();

        public static MediaEntity ConvertToDto(this MediaDO model)
        {
            return mapper_MediaDO_2_MediaEntity.Map(model);
        }

        public static MediaDO ConvertToModel(this MediaEntity entity)
        {
            return mapper_MediaEntity_2_MediaDO.Map(entity);
        }

        #endregion

        #region Media Task

        private static readonly ObjectsMapper<MediaTaskEntity, MediaTaskDO> mapper_MediaTaskEntity_2_MediaTaskDO = ObjectMapperManager.DefaultInstance.GetMapper<MediaTaskEntity, MediaTaskDO>();

        private static readonly ObjectsMapper<MediaTaskDO, MediaTaskEntity> mapper_MediaTaskDO_2_MediaTaskEntity = ObjectMapperManager.DefaultInstance.GetMapper<MediaTaskDO, MediaTaskEntity>();

        public static MediaTaskEntity ConvertToDto(this MediaTaskDO model)
        {
            return mapper_MediaTaskDO_2_MediaTaskEntity.Map(model);
        }

        public static MediaTaskDO ConvertToModel(this MediaTaskEntity entity)
        {
            return mapper_MediaTaskEntity_2_MediaTaskDO.Map(entity);
        }

        #endregion

        #region DownLoad

        private static readonly ObjectsMapper<DownloadEntity, DownloadDO> mapper_DownloadEntity_2_DownloadDO = ObjectMapperManager.DefaultInstance.GetMapper<DownloadEntity, DownloadDO>();

        private static readonly ObjectsMapper<DownloadDO, DownloadEntity> mapper_DownloadDO_2_DownloadEntity = ObjectMapperManager.DefaultInstance.GetMapper<DownloadDO, DownloadEntity>();

        public static DownloadEntity ConvertToDto(this DownloadDO model)
        {
            return mapper_DownloadDO_2_DownloadEntity.Map(model);
        }

        public static DownloadDO ConvertToModel(this DownloadEntity entity)
        {
            return mapper_DownloadEntity_2_DownloadDO.Map(entity);
        }

        #endregion
    }
}
