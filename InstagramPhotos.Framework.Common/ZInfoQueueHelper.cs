using InstagramPhotos.Utility.Configuration;
using InstagramPhotos.ViewModel;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Service.BLL;
using Service.Interface.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ViewModel;

namespace InstagramPhotos.Framework.Common
{
    public class ZInfoQueueHelper
    {
        private readonly Guid sys = Guid.NewGuid();
        private readonly IMediaService mediaService = ServiceFactory.GetInstance<IMediaService>();
        private readonly string baseHttpUrl =AppSettings.GetValue<string>("IndexUrl");

        public readonly static ZInfoQueueHelper Instance = new ZInfoQueueHelper();
        private ZInfoQueueHelper()
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
                            ex.Message.Log();
                        }
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        "当前任务全部解析已完成".Log(true);
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

                    SaveFromExtraFile(model.Url, model.TaskId);
                }
                catch (Exception e)
                {
                    e.Message.Log();
                }
            }
        }

       

        /// <summary>
        /// 解析文件所含的全部资源
        /// </summary>
        /// <param name="url"></param>
        public string SaveFromExtraFile(string url, Guid taskId)
        {
            using (var driver = new PhantomJSDriver(GetPhantomJSDriverService()))
            {
                $"正在加载地址：{url}...".Log(true);
                driver.Navigate().GoToUrl(url);
                //最大化窗口
                MaxBrowser(driver);
                "正在调整浏览器窗体大小...".Log(true);
                //第1次加载更多
                //用户的分享超过一屏时
                IWebElement moreShare = null;
                try
                {
                    moreShare = driver.FindElement(By.LinkText("更多"));
                }
                catch
                {
                    "该用户可分享的内容少于一屏，跳过流程解析...".Log(true);
                }
                if (moreShare != null)
                {
                    "正在点击页面的“更多”按钮...".Log(true);
                    driver.FindElement(By.LinkText("更多")).Click();
                    System.Threading.Thread.Sleep(2000);
                    "正在加载第二页内容...".Log(true);
                    "第二页内容加载完成...".Log(true);
                    //获取需要滚动加载的次数
                    "正在计算全部需要加载的页数...".Log(true);
                    var cardCount = int.Parse(driver.FindElements(By.CssSelector("._fd86t")).First().Text.Replace(",", ""));
                    var scrollCount = cardCount / 12 == 0 ? cardCount / 12 : (cardCount / 12) + 1;
                    $"计算完成，全部需要加载{scrollCount}页...".Log(true);
                    //默认滚动条移动到y轴3000的位置
                    var initialC = 3000;
                    var i = 0;
                    "开始执行脚本，滚动鼠标...".Log(true);
                    do
                    {
                        //if (i > 20)//预设只滚动20次，太多次容易卡顿
                        //    break;
                        $"第{i + 1}次滚动...".Log(true);
                        ((IJavaScriptExecutor)driver).ExecuteScript($"scrollTo(0,{initialC});");
                        System.Threading.Thread.Sleep(1000 + (i * 50));
                        i++;
                        initialC += 500;//每次递增滚动500的距离
                    } while (i < scrollCount);
                }
                "页面所有内容全部加载完成...".Log(true);

                var insDir = url.Substring(url.LastIndexOf('/') + 1);

                "正在解析下载的资源...".Log(true);
                ExtraDownloadSource(driver.PageSource, taskId);
                driver.Quit();
                return insDir;
            }
        }

        /// <summary>
        /// 解析出需要下载的资源数并添加到数据库
        /// </summary>
        /// <param name="htmlFilePath">已经解析的html文件</param>
        public void ExtraDownloadSource(string html, Guid taskId)
        {
            //解析出资源
            var regExt = new Regex("\\<a\\s*href\\=\"(?<href>\\/p\\/\\S+\\/)\"\\>");
            var matchs = regExt.Matches(html);

            var task = mediaService.GetMediatask(taskId);
            if (matchs != null && matchs.Count > 0)
            {
                for (int i = 0; i < matchs.Count; i++)
                {
                    //明细页面路径
                    var url = baseHttpUrl + matchs[i].Groups["href"].Value;
                    using (var driver = new PhantomJSDriver(PhantomJSDriverService.CreateDefaultService()))
                    {
                        //解析文件
                        $"正在浏览资源:{url}".Log(true);
                        driver.Navigate().GoToUrl(url);
                        var htmlDetail = driver.PageSource;
                        IList<IWebElement> mediaUrls = new List<IWebElement>();
                        try
                        {
                            var img = driver.FindElementByCssSelector("._2di5p");
                            if (img != null)
                            {
                                mediaUrls.Add(img);
                                "找到资源所在的唯一标签".Log(true);
                            }
                        }
                        catch
                        {
                            mediaUrls = driver.FindElements(By.TagName("img"));
                            "找到资源的img标签".Log(true);
                        }
                        driver.Quit();
                        foreach (var item in mediaUrls)
                        {
                            var mediaUrl = item.GetAttribute("src");
                            $"上传下载任务：{mediaUrl}".Log(true);
                            mediaService.AddDownload(new DownloadEntity
                            {
                                DownloadId = Guid.NewGuid(),
                                SortValue = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                                Disabled = 0,
                                Rec_CreateBy = Guid.Empty,
                                Rec_CreateTime = DateTime.Now,
                                Rec_ModifyBy = Guid.Empty,
                                Rec_ModifyTime = DateTime.Now,
                                HttpUrl = mediaUrl,
                                DirName = task.Url.Substring(task.Url.LastIndexOf('/') + 1)
                            });
                        }

                    }
                }
                "该用户全部任务收集完毕!".Log(true);
                task.Disabled = 1;
                mediaService.UpdateMediatask(task);
                "更新用户资源解析任务的状态为已解析...".Log(true);
            }
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        /// <returns></returns>
        private static PhantomJSDriverService GetPhantomJSDriverService()
        {
            PhantomJSDriverService pds = PhantomJSDriverService.CreateDefaultService();
            //设置代理服务器地址
            //pds.Proxy = $"{ip}:{port}";  
            //设置代理服务器认证信息
            //pds.ProxyAuthentication = GetProxyAuthorization();
            return pds;
        }

        /// <summary>
        /// 将浏览器最大化
        /// </summary>
        /// <param name="driver"></param>
        public void MaxBrowser(IWebDriver driver)
        {
            try
            {
                var maxBrowser = @"if (window.screen) {window.moveTo(0, 0);
                        window.resizeTo(window.screen.availWidth,window.screen.availHeight);}";
                var jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript(maxBrowser);
            }
            catch (Exception)
            {
                throw new Exception("Fail to  Maximization browser");
            }
        }
    }
}
