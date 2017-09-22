using InstagramPhotos.QueryModel;
using InstagramPhotos.Task.Consoles;
using InstagramPhotos.Utility.KVStore;
using Service.BLL;
using Service.Interface.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InstagramPhotos.Task.Download
{
    class Program
    {
        private readonly Guid sys = Guid.NewGuid();
        private readonly static IMediaService mediaService = ServiceFactory.GetInstance<IMediaService>();

        static void Main(string[] args)
        {
            KVStoreManager.SetEngine(new KVStoreEngine());
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            while (true)
            {
                List<ViewModel.DownloadEntity> downloadTasks = null;
                //如果出现异常，重试4次
                var queryFlag = 0;
                for (int i = 0; i < 5; i++)
                {
                    try
                    {
                        if (queryFlag <= 0)
                        {
                            var downQo = new DownloadQO();
                            downQo.Equal(DownloadQO.QueryEnums.Disabled, 0).OrderByAsc("Disabled");
                            downloadTasks = mediaService.GetDownloadDtosByPara(downQo, false);
                            if (downloadTasks == null || !downloadTasks.Any())
                            {
                                Console.WriteLine("当前批次下载任务已完成，等待10秒继续扫描待下载的任务...".Log());
                                System.Threading.Thread.Sleep(10000);
                                continue;
                            }
                            queryFlag += 1;
                        }
                        else
                        {
                            break;
                        }
                        if (i == 4)
                        {
                            Console.WriteLine("当前程序已重试4次，未正常获取到数据，程序正在退出...".Log());
                            Environment.Exit(-1);//退出
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"查询下载任务出现异常，异常信息:{e.Message}，正在进行第{i + 1}次重试...".Log());
                        queryFlag -= 1;
                    }
                }


                //生成下载任务，并保存到指定目录
                for (int i = 0; i < downloadTasks.Count; i++)
                {
                    try
                    {
                        Console.WriteLine($"[正在下载资源：{downloadTasks[i].HttpUrl}][{DateTime.Now}]".Log());
                        // 设置参数
                        var httpUrl = downloadTasks[i].HttpUrl;
                        Stream rps = null;
                        try
                        {
                            HttpWebRequest rq = WebRequest.Create(httpUrl) as HttpWebRequest;
                            //发送请求并获取相应回应数据
                            HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
                            //直到request.GetResponse()程序才开始向目标网页发送Post请求
                            rps = rp.GetResponseStream();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"下载出现异常：{e.Message}".Log());
                            continue;
                        }

                        //目标目录
                        var insDir = Environment.CurrentDirectory + "\\" + downloadTasks[i].DirName;
                        //创建本地文件写入流
                        if (!Directory.Exists(insDir))
                        {
                            Directory.CreateDirectory(insDir);
                        }
                        //网络资源文件是否已下载
                        var fileReg = new Regex("(\\d+_){3}\\w*.(jpg|jpeg|png|mp4|flv|gif)");
                        var sourceFileName = fileReg.Match(httpUrl).Value;

                        //校验目标目录中的文件是否已存在，如果存在则跳过，否则下载
                        if (File.Exists(insDir + "\\" + sourceFileName))
                        {
                            Console.WriteLine($"[{sourceFileName}][此资源已下载，跳过]!".Log());
                            continue;
                        }

                        Stream st = new FileStream(insDir + $"\\{sourceFileName}", FileMode.Create);
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
                        downloadTasks[i].Disabled = 1;
                        mediaService.UpdateDownload(downloadTasks[i]);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                Console.WriteLine($"当前批次下载任务已完成，等待10秒继续扫描待下载的任务...[{DateTime.Now}]".Log());
                System.Threading.Thread.Sleep(10000);
            }
        }

        //控制台退出时
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {

        }
    }
}
