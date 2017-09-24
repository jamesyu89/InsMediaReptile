using System;
using System.Collections.Generic;
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
            HttpWebRequest rq = WebRequest.Create(httpUrl) as HttpWebRequest;
            rq.Timeout = 1000 * 15;//超时时间设置了30秒
            //发送请求并获取相应回应数据
            HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            var rps = rp.GetResponseStream();

            Stream st = new FileStream(filePath, FileMode.Create);
            byte[] bar = new byte[1024];
            int sz = rps.Read(bar, 0, (int)bar.Length);
            while (sz > 0)
            {
                st.Write(bar, 0, sz);
                sz = rps.Read(bar, 0, (int)bar.Length);
            }
            st.Close();
            rps.Close();
        }

        /// <summary>
        /// 获取当前网络地址加载的html(如果失败，会重试5次)
        /// </summary>
        /// <param name="httpUrl"></param>
        /// <returns></returns>
        public static string GetHttpUrlString(string httpUrl)
        {
            var html = string.Empty;
            Action action = () =>
            {
                var webRequest = WebRequest.Create(httpUrl);
                var response = webRequest.GetResponse();
                var stream = response.GetResponseStream();
                var reader = new StreamReader(stream);
                html = reader.ReadToEnd();
                reader.Close();
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
