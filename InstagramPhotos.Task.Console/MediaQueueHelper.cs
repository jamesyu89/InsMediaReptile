using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace InstagramPhotos.Task.Consoles
{
    /// <summary>
    /// 队列临时类  
    /// </summary>
    public class MediaInfo
    {
        /// <summary>
        /// Ins用户名
        /// </summary>
        public string InsName { get; set; }
        /// <summary>
        /// 资源文件网络路径
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 要保存到文件(html)的(包含路径的)全名
        /// </summary>
        public string FileFullName
        {
            get
            {
                return DefaultPath + $"\\{InsName}.html";
            }
        }
        /// <summary>
        /// 文件资源类型,逗号隔开
        /// </summary>
        public string MetaTypeList { get; set; }
        /// <summary>
        /// 匹配资源的正则表达式,逗号隔开
        /// </summary>
        public string RegexList { get; set; }
        /// <summary>
        /// 默认路径
        /// </summary>
        public string DefaultPath
        {
            get
            {
                return Environment.CurrentDirectory + "\\" + this.InsName;
            }
        }
    }

    /// <summary>
    /// 队列处理类
    /// </summary>
    public class MediaQueueHelper
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
                    // 设置参数
                    HttpWebRequest request = WebRequest.Create(model.Url) as HttpWebRequest;
                    //发送请求并获取相应回应数据
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    //直到request.GetResponse()程序才开始向目标网页发送Post请求
                    Stream responseStream = response.GetResponseStream();
                    //创建本地文件写入流
                    if (!Directory.Exists(model.DefaultPath))
                    {
                        Directory.CreateDirectory(model.DefaultPath);
                    }
                    Stream stream = new FileStream(model.FileFullName, FileMode.OpenOrCreate);
                    byte[] bArr = new byte[1024];
                    int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    while (size > 0)
                    {
                        stream.Write(bArr, 0, size);
                        size = responseStream.Read(bArr, 0, (int)bArr.Length);
                    }
                    stream.Close();
                    responseStream.Close();

                    //todo:解析文件，生成任务
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
                    Parallel.For(0, matchs.Count, (i) =>
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
                                //创建本地文件写入流
                                var fileType = httpUrl.Substring(httpUrl.LastIndexOf('.'));
                                Stream st = new FileStream(path + "\\" + Guid.NewGuid() + fileType, FileMode.Create);
                                byte[] bar = new byte[1024];
                                int sz = rps.Read(bar, 0, (int)bar.Length);
                                while (sz > 0)
                                {
                                    st.Write(bar, 0, sz);
                                    sz = rps.Read(bar, 0, (int)bar.Length);
                                }
                                st.Close();
                                rps.Close();
                                Console.WriteLine("[资源下载成功！]");
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        });
                    });
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}