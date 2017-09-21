using InstagramPhotos.Media.QueryModel;
using InstagramPhotos.Media.ViewModel;
using InstagramPhotos.Utility.KVStore;
using Service.BLL;
using Service.Interface.Media;
using System;
using System.Collections.Generic;
using ViewModel;

namespace InstagramPhotos.Task.Consoles
{
    class Program
    {
        static readonly IMediaService mediaService = ServiceFactory.GetInstance<IMediaService>();

        static void Main(string[] args)
        {
            //初始化缓存引擎
            KVStoreManager.SetEngine(new KVStoreEngine());
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            while (true)
            {
                Console.WriteLine("是否立即扫描数据库并开始下载资源(y/n)?");
                var k = Console.ReadLine();
                if (k.ToUpper() == "Y")
                {
                    Console.WriteLine("========================================================");
                    Console.WriteLine("正在初始化...");
                    Console.WriteLine("检查需要被执行的任务...");
                    var mediaTaskQo = new MediaTaskQO();
                    mediaTaskQo.Equal(MediaTaskQO.QueryEnums.Disabled, 0);
                    var mediaResult = mediaService.GetMediataskDtosByPara(mediaTaskQo, false);
                    var taskRemainCount = mediaResult != null ? mediaResult.Count : 0;
                    Console.WriteLine($"当前任务数为{taskRemainCount}...");
                    if (taskRemainCount > 0)
                    {
                        Console.WriteLine("正在将任务添加至队列中....");
                        mediaResult.ForEach(m =>
                        {
                            MediaQueueHelper.Instance.AddQueue(m.Url,m.MediaTaskId);
                        });
                        Console.WriteLine("添加完成....");
                    }
                    Console.WriteLine("初始化结束...");
                    Console.WriteLine("处理队列中....");
                    MediaQueueHelper.Instance.Start();
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(Environment.NewLine);
                }
            }

            //移交控制权
            //while (true)
            //{
            //    ShowInput();
            //    var name = Console.ReadLine();
            //    if (string.IsNullOrEmpty(name))
            //    {
            //        Console.WriteLine("无效的字符");
            //        continue;
            //    }
            //    MediaQueueHelper.Instance.AddQueue("jpg,png,gif,mp4,mov", @"http(s?):\/\/\w+(.)\w+(.)+\/.+\w+", name);
            //    Console.WriteLine($"正在加入到处理队列...");
            //    Console.WriteLine($"队列正在处理任务...");
            //    Console.WriteLine($"前面还有{(MediaQueueHelper.Instance.ListCount == 0 ? 0 : MediaQueueHelper.Instance.ListCount - 1)}个任务等待处理...");
            //    Console.WriteLine($"========================================================");
            //    Console.WriteLine($"========================================================");
            //}
        }





        //控制台退出时
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            if (MediaQueueHelper.Instance.ListCount > 0)
            {
                //写入到数据库中
                var mediaList = new List<MediaTaskEntity>();
                var flag = true;
                MediaInfo media = null;
                while (flag)
                {
                    try
                    {
                        media = MediaQueueHelper.Instance.Dequeue();
                        if (media != null)
                        {
                            mediaList.Add(new MediaTaskEntity
                            {
                                Disabled = 0,
                                FileFullName = media.FileFullName,
                                MediaTaskId = Guid.NewGuid(),
                                MetaTypeList = media.MetaTypeList,
                                Rec_CreateBy = Guid.Empty,
                                Rec_CreateTime = DateTime.Now,
                                Rec_ModifyBy = Guid.Empty,
                                Rec_ModifyTime = DateTime.Now,
                                RegexList = media.RegexList,
                                Url = media.Url
                            });
                        }
                    }
                    catch (Exception)
                    {
                        flag = false;
                    }
                }
                mediaService.BatchAddMediatask(mediaList);
            }
        }
    }
}
