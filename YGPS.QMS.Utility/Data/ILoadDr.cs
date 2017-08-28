using System.Data;

namespace InstagramPhotos.Utility.Data
{
    public interface ILoadDr
    {
        void LoadData(IDataReader dr);
    }
}
