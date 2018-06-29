using InstagramPhotos.ViewModel;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Service.BLL;
using Service.Interface.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace InstagramPhotos.Framework.Common
{
    /// <summary>
    /// 媒体文件的下载与资源解析队列类
    /// </summary>
    public partial class MediaQueueHelper
    {
        private readonly Guid sys = Guid.NewGuid();
        private readonly IMediaService mediaService = ServiceFactory.GetInstance<IMediaService>();
        private readonly string baseHttpUrl = "https://www.instagram.com";

        /// <summary>
        /// 解析文件所含的全部资源
        /// </summary>
        /// <param name="url"></param>
        public string SaveFromExtraFile(string url, Guid taskId)
        {
            using (var driver = new PhantomJSDriver(GetPhantomJSDriverService()))
            {
                try
                {
                    $"正在加载地址：{url}...".Log(true);
                    driver.Navigate().GoToUrl(url);
                    //最大化窗口
                    MaxBrowser(driver);
                    "调整浏览器窗体为最大化...".Log(true);
                    //如果有“更多”按钮
                    IWebElement moreShare = null;
                    try
                    {
                        moreShare = driver.FindElement(By.LinkText("更多"));
                    }
                    catch
                    {
                        "页面上没有找到“更多”按钮...".Log(true);
                        "尝试直接滚动鼠标以加载分页内容...".Log(true);
                    }
                    if (moreShare != null)
                    {
                        "正在点击页面的“更多”按钮...".Log(true);
                        driver.FindElement(By.LinkText("更多")).Click();
                        System.Threading.Thread.Sleep(2000);
                    }

                    //获取需要滚动加载的次数
                    "正在计算全部需要加载的页数...".Log(true);
                    var cardCount = int.Parse(driver.FindElements(By.CssSelector(".g47SY")).First().Text.Replace(",", ""));
                    var scrollCount = cardCount / (3 * 8) == 0 ? cardCount / (3 * 8) : (cardCount / (3 * 8)) + 1;
                    $"计算完成，全部需要加载{scrollCount}页...".Log(true);
                    //默认第一次滚动条移动到y轴1900的位置,刚好翻一页
                    var initialC = 1900;
                    var i = 0;
                    "开始执行脚本，滚动鼠标...".Log(true);
                    do
                    {
                        $"第{i + 1}次滚动鼠标...".Log(true);
                        ((IJavaScriptExecutor)driver).ExecuteScript($"scrollTo(0,{initialC});");
                        $"Y轴当前位置{initialC}".Log(true);
                        System.Threading.Thread.Sleep(1000 + (i * 50));
                        i++;
                        initialC += 500;//每次递增滚动500的距离
                    } while (i < scrollCount);
                    "页面所有内容全部加载完成...".Log(true);

                    var insDir = url.Substring(url.LastIndexOf('/') + 1);
                    "正在解析下载的资源...".Log(true);
                    ExtraDownloadSource(driver.PageSource, taskId);
                    return insDir;
                }
                catch (Exception e)
                {
                    e.Message.Log(true);
                    throw e;
                }
                finally
                {
                    driver.Quit();
                }
            }
        }

        /// <summary>
        /// 解析出需要下载的资源数并添加到数据库
        /// </summary>
        /// <param name="htmlFilePath">已经解析的html文件</param>
        public void ExtraDownloadSource(string html, Guid taskId)
        {
            //解析出资源
            var regExt = new Regex("\\<a\\s*href\\=\"(?<href>\\/p\\/\\S+)\"\\>");
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
            pds.HideCommandPromptWindow = true;
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
