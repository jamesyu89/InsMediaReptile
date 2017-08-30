namespace InstagramPhotos.Utility.Storage
{
    public interface IStorageEngine
    {
        string StoreImage(string file_name, byte[] image_data, string dir_name = "guanplus/temp/img",
            string content_type = "image/jpeg");
    }
}