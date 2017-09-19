using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ViewModel;

namespace InstagramPhotos.Task.Consoles
{
    /// <summary>
    /// 队列处理类
    /// </summary>
    public partial class MediaQueueHelper
    {
        public readonly static MediaQueueHelper Instance = new MediaQueueHelper();
        private MediaQueueHelper()
        { }
        /// <summary>
        /// 队列当前剩余任务量
        /// </summary>
        public int ListCount
        {
            get { return ListQueue.Count; }
        }
        private Queue<MediaInfo> ListQueue = new Queue<MediaInfo>();

        /// <summary>
        /// 入列
        /// </summary>
        /// <param name="metaTypeList"></param>
        /// <param name="regexList"></param>
        public void AddQueue(string metaTypeList, string regexList, string insName)
        {
            MediaInfo queueinfo = new MediaInfo
            {
                InsName = insName,
                MetaTypeList = metaTypeList,
                RegexList = regexList,
                Url = $"https://www.instagram.com/{insName}"
            };
            ListQueue.Enqueue(queueinfo);
        }

        public void AddQueue(string url)
        {
            MediaInfo queueinfo = new MediaInfo
            {
                InsName = "",
                MetaTypeList = url.Substring(url.IndexOf('.', 0)),//.jpg、.png
                RegexList = "",
                Url = url
            };
            ListQueue.Enqueue(queueinfo);
        }

        /// <summary>
        /// 出列
        /// </summary>
        /// <returns></returns>
        public MediaInfo Dequeue()
        {
            return ListQueue.Dequeue();
        }

        public void Start()//启动  
        {
            System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() =>
            {
                while (true)
                {
                    if (ListQueue.Count > 0)
                    {
                        try
                        {
                            ScanQueue();
                        }
                        catch (Exception ex)
                        {
                            //(ex.ToString());
                        }
                    }
                    else
                    {
                        //没有任务，休息3秒钟  
                        Thread.Sleep(3000);
                    }
                }
            });
            task.Start();
        }

        //要执行的方法  
        private void ScanQueue()
        {
            while (ListQueue.Count > 0)
            {
                try
                {
                    //从队列中取出  
                    MediaInfo model = ListQueue.Dequeue();

                    SaveFromExtraFile(model.Url, model.InsName);

                    //文件存放位置
                    var path = Environment.CurrentDirectory + $"\\{model.InsName}";
                    var directoryInfo = new DirectoryInfo(path);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //解析出下载资源数
                    StreamReader reader = new StreamReader(new FileStream(model.FileFullName, FileMode.Open));
                    var html = reader.ReadToEnd();
                    reader.Close();
                    var reg = new Regex("\"thumbnail_src\"\\s*:\\s*\"(?<media>http(s)?:\\/\\/\\S+.(jpg|png|mp4|flv))\"");
                    var matchs = reg.Matches(html);

                    if (matchs == null || matchs.Count == 0)
                        return;

                    //生成下载任务，并保存到指定目录
                    var result = Parallel.For(0, matchs.Count, (i) =>
                     {
                         System.Threading.Tasks.Task.Run(() =>
                         {
                             try
                             {
                                 Console.WriteLine("[正在下载资源：" + matchs[i].Groups["media"].Value + "]");
                                // 设置参数
                                var httpUrl = matchs[i].Groups["media"].Value;
                                 HttpWebRequest rq = WebRequest.Create(httpUrl) as HttpWebRequest;
                                //发送请求并获取相应回应数据
                                HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
                                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                                Stream rps = rp.GetResponseStream();

                                //网络资源文件是否已下载
                                var fileReg = new Regex("(\\d+_){3}\\w*.(jpg|jpeg|png|mp4|flv|gif)");
                                 var sourceFileName = fileReg.Match(httpUrl).Value;
                                //校验目标目录中的文件是否已存在，如果存在则跳过，否则下载
                                if (File.Exists(sourceFileName))
                                 {
                                     Console.WriteLine("[此资源已下载，跳过]!");
                                     return;
                                 }

                                //创建本地文件写入流
                                Stream st = new FileStream(path + $"\\{sourceFileName}", FileMode.Create);
                                 byte[] bar = new byte[1024];
                                 int sz = rps.Read(bar, 0, (int)bar.Length);
                                 while (sz > 0)
                                 {
                                     st.Write(bar, 0, sz);
                                     sz = rps.Read(bar, 0, (int)bar.Length);
                                 }
                                 st.Close();
                                 rps.Close();
                                 Console.WriteLine("[资源下载完成！]");
                             }
                             catch (Exception)
                             {
                                 throw;
                             }
                         });
                     });
                    if (result.IsCompleted)
                    {
                        Console.WriteLine("=================输入Instagram用户名即可================");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 下载网络文件
        /// </summary>
        /// <param name="defaultPath">本地默认存放目录</param>
        /// <param name="httpUrl">资源url</param>
        /// <param name="phycialPath">文件物理路径</param>
        private void SaveNetFile(string defaultPath, string httpUrl, string phycialPath)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(httpUrl) as HttpWebRequest;
            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream responseStream = response.GetResponseStream();
            //创建本地文件写入流
            if (!Directory.Exists(defaultPath))
            {
                Directory.CreateDirectory(defaultPath);
            }
            Stream stream = new FileStream(phycialPath, FileMode.OpenOrCreate);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
        }
    }
}