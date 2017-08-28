namespace InstagramPhotos.Utility.FastReflection
{
    public interface IFastReflectionFactory<TKey, TValue>
    {
        TValue Create(TKey key);
    }
}
