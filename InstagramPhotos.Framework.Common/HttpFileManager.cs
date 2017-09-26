using InstagramPhotos.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPhotos.Framework.Common
{
    public class HttpFileManager
    {
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <param name="filePath"></param>
        public static void DownloadFile(string httpUrl, string filePath)
        {
            ServicePointManager.DefaultConnectionLimit = int.Parse(AppSettings.GetValue<string>("DefaultConnectionLimit", "512"));
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(httpUrl);
            req.ServicePoint.Expect100Continue = false;
            req.ServicePoint.UseNagleAlgorithm = false;
            req.ServicePoint.ConnectionLimit = 65500;
            req.AllowWriteStreamBuffering = false;
            req.Proxy = null;
            req.Method = "GET";
            //req.KeepAlive = true;
            req.ContentType = "image/*";
            req.Timeout = 1000 * 100;
            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Stream stream = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                Image.FromStream(stream).Save(filePath);
            }
            finally
            {
                // 释放资源
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        /// <summary>
        /// 获取当前网络地址加载的html(如果失败，会重试5次)
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public static string GetHttpUrlString(string httpUrl)
        {
            GC.Collect();//回收一次垃圾，保证后续的线程能正常启动连接
            ServicePointManager.DefaultConnectionLimit = int.Parse(AppSettings.GetValue<string>("DefaultConnectionLimit", "512"));
            var html = string.Empty;
            Action action = () =>
            {
                HttpWebRequest webRequest = WebRequest.Create(httpUrl) as HttpWebRequest;
                webRequest.Method = "GET";
                webRequest.UserAgent = "Opera/9.25 (Windows NT 6.0; U; en)";
                webRequest.Timeout = 1000 * 30;//超时时间设置了30秒
                webRequest.KeepAlive = true;
                var response = webRequest.GetResponse();
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);
                html = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                response.Close();
            };
            var flag = 0;
            for (int j = 0; j < 5; j++)
            {
                try
                {
                    if (flag <= 0)
                    {
                        action();
                        return html;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    flag = -1;
                    e.Message.Log();
                }
            }
            return html;
        }
    }
}
