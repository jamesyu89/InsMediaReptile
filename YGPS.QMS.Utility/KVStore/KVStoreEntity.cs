namespace InstagramPhotos.Utility.KVStore
{
    public class KVStoreEntity
    {
    }

    public class CasResult<T>
    {
        public ulong Version { get; set; }

        public T Result { get; set; }

        public bool IsSuccess { get; set; }
    }
}
