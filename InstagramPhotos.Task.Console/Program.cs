using InstagramPhotos.Framework.Common;
using InstagramPhotos.Media.QueryModel;
using InstagramPhotos.Media.ViewModel;
using InstagramPhotos.Utility.KVStore;
using InstagramPhotos.ViewModel;
using Service.BLL;
using Service.Interface.Media;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModel;


namespace InstagramPhotos.Task.Consoles
{
    class Program
    {
        static readonly IMediaService mediaService = ServiceFactory.GetInstance<IMediaService>();
        private readonly static Guid sys = Guid.Parse("3102A7AC-35DF-4C9C-8A11-CE9501EBE300");
        static void Main(string[] args)
        {
            //初始化缓存引擎
            KVStoreManager.SetEngine(new KVStoreEngine());

            while (true)
            {
                if (MediaQueueHelper.Instance.ListCount == 0)
                {
                    Console.WriteLine("========================================================".Log());
                    Console.WriteLine("正在初始化...".Log());
                    Console.WriteLine("检查需要被执行的任务...".Log());
                    var mediaTaskQo = new MediaTaskQO();
                    mediaTaskQo.Equal(MediaTaskQO.QueryEnums.Disabled, 0);
                    var mediaResult = mediaService.GetMediataskDtosByPara(mediaTaskQo, false);
                    var taskRemainCount = mediaResult != null ? mediaResult.Count : 0;
                    Console.WriteLine($"当前任务数为{taskRemainCount}...".Log());
                    if (taskRemainCount > 0)
                    {
                        Console.WriteLine("正在将任务添加至队列中....".Log());
                        mediaResult.ForEach(m =>
                        {
                            MediaQueueHelper.Instance.AddQueue(m.Url, m.MediaTaskId);
                        });
                        Console.WriteLine("添加完成....".Log());
                    }
                    Console.WriteLine("初始化结束...".Log());
                    Console.WriteLine("处理队列中....".Log());
                    MediaQueueHelper.Instance.Start();
                    Console.WriteLine(Environment.NewLine);
                    Console.WriteLine(Environment.NewLine);
                }
            }
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
