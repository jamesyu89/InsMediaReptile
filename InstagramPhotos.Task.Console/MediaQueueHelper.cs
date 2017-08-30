using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
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
                return DefaultPath + $"/{InsName}.html";
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
                return Environment.CurrentDirectory;
            }
        }
    }

    /// <summary>
    /// 队列处理类
    /// </summary>
    public class MediaQueueHelper
    {
        //#region 解决发布时含有优质媒体时，前台页面卡住的现象  
        //原理：利用生产者消费者模式进行入列出列操作  

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
                    Stream stream = new FileStream(model.FileFullName, FileMode.Create);
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
                    
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}