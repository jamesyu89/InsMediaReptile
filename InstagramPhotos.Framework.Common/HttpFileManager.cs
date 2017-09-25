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
            while (true)
            {
                try
                {
                    ServicePointManager.DefaultConnectionLimit = int.MaxValue;
                    HttpWebRequest rq = WebRequest.Create(httpUrl) as HttpWebRequest;
                    rq.Method = "GET";
                    rq.UserAgent = "Opera/9.25 (Windows NT 6.0; U; en)";
                    rq.KeepAlive = true;
                    HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
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
                    rp.Close();
                    break;
                }
                catch (Exception e)
                {
                    e.StackTrace.ToString();
                    System.Diagnostics.Trace.WriteLine(e.Message);
                    if (true)
                        continue;
                }
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
            ServicePointManager.DefaultConnectionLimit = 200;
            var html = string.Empty;
            Action action = () =>
            {
                ServicePointManager.DefaultConnectionLimit = int.MaxValue;
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
