using InstagramPhotos.DomainModel;
using InstagramPhotos.Media.DomainModel;
using InstagramPhotos.Utility.KVStore;
using System;

namespace Service.BLL.Media
{
    internal class MediaCommon
    {
        internal static readonly KVStoreEntityTable<Guid, MediaDO> cache_Media = KVStoreManager.GetKVStoreEntityTable<Guid, MediaDO>("InstagramPhotos.Media.DomainModel._Media");
        internal static readonly KVStoreEntityTable<Guid, MediaTaskDO> cache_MediaTask = KVStoreManager.GetKVStoreEntityTable<Guid, MediaTaskDO>("InstagramPhotos.Media.DomainModel._MediaTask");
        internal static readonly KVStoreEntityTable<Guid, DownloadDO> cache_Download = KVStoreManager.GetKVStoreEntityTable<Guid, DownloadDO>("InstagramPhotos.DomainModel._Download");
        internal static readonly KVStoreEntityTable<Guid, DownloadLogDO> cache_DownloadLog = KVStoreManager.GetKVStoreEntityTable<Guid, DownloadLogDO>("InstagramPhotos.DomainModel._DownloadLog");
    }
}
