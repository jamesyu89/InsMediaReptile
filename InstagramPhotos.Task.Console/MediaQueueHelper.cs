using InstagramPhotos.Media.QueryModel;
using InstagramPhotos.QueryModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

        public void AddQueue(string url, Guid taskId)
        {
            MediaInfo queueinfo = new MediaInfo
            {
                InsName = "",
                MetaTypeList = url.Substring(url.IndexOf('.', 0)),//.jpg、.png
                RegexList = "",
                Url = url,
                TaskId = taskId
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
                        Thread.Sleep(3000);
                       // Console.WriteLine("别让我闲着！");
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

                    var insDir = SaveFromExtraFile(model.Url,model.TaskId);
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
        private void SaveNetFile(string defaultPath, string builder, string phycialPath)
        {
            //创建本地文件写入流
            if (!Directory.Exists(defaultPath))
            {
                Directory.CreateDirectory(defaultPath);
            }
            var encoding = Encoding.UTF8.GetBytes(builder);
            Stream stream = new FileStream(phycialPath, FileMode.OpenOrCreate);
            stream.Write(encoding, 0, builder.Length);
            stream.Close();
        }


    }
}