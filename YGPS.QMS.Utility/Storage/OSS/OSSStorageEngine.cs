using System;
using System.IO;
using Aliyun.OpenServices.OpenStorageService;
using InstagramPhotos.Utility.Configuration;
using InstagramPhotos.Utility.IO;
using InstagramPhotos.Utility.Log;

namespace InstagramPhotos.Utility.Storage.OSS
{
    public class OSSStorageEngine : IStorageEngine
    {
        #region [        Members       ]

        private readonly string config_oss_access_id = AppSettings.GetValue("oss_access_id", "DBxXreRXFgDjyTxW");

        private readonly string config_oss_access_key = AppSettings.GetValue("oss_access_key",
            "9uUVDl4JscJN4RRfZ0EOiglLij0srB");

        private readonly string config_oss_bucket = AppSettings.GetValue("oss_bucket", "shccjoy");

        private readonly string config_oss_endpoint = AppSettings.GetValue("oss_endpoint",
            "http://oss-cn-hangzhou.aliyuncs.com");


        private readonly string config_oss_temp_path = AppSettings.GetValue("config_oss_temp_path", @"C:\oss_temp_path\");

        #endregion

        #region [    IStorageEngine    ]

        public string StoreImage(string file_name, byte[] image_data, string dir_name,
            string content_type)
        {
            string key = string.Empty;

            try
            {
                var ossClient = new OssClient(config_oss_endpoint, config_oss_access_id, config_oss_access_key);
                var meta = new ObjectMetadata();
                meta.ContentType = content_type;
                key = string.Format("{0}/{1}", dir_name, file_name);

                PutObjectResult result = ossClient.PutObject(config_oss_bucket, key, new MemoryStream(image_data), meta);
                    //上传图片
                Logger.Info("保存图片{0}到OSS", file_name);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "保存图片{0}到OSS失败，将保存到暂存区{1}", file_name, config_oss_temp_path+dir_name);

                try
                {
                    FileOperationHelper.Save(image_data, config_oss_temp_path, file_name);
                }
                catch (Exception)
                {
                    Logger.Exception(ex, "保存图片{0}到暂存区失败", file_name);
                }
            }

            return string.Format("http://{0}.oss-cn-hangzhou.aliyuncs.com/{1}", config_oss_bucket, key);
        }

        #endregion
    }
}