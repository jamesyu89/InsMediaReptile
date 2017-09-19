using HtmlAgilityPack;
using InstagramPhotos.ViewModel;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Service.BLL;
using Service.Interface.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InstagramPhotos.Task.Consoles
{
    /// <summary>
    /// 媒体文件的下载与资源解析队列类
    /// </summary>
    public partial class MediaQueueHelper
    {
        private readonly Guid sys = Guid.NewGuid();
        private readonly IMediaService mediaService = ServiceFactory.GetInstance<IMediaService>();

        /// <summary>
        /// 解析文件所含的全部资源并保存
        /// </summary>
        /// <param name="url"></param>
        public void SaveFromExtraFile(string url, string insName)
        {
            IWebDriver driver = new PhantomJSDriver(GetPhantomJSDriverService());
            driver.Navigate().GoToUrl(url);
            //最大化窗口
            MaxBrowser(driver);

            //第1次加载更多
            driver.FindElement(By.LinkText("更多")).Click();
            System.Threading.Thread.Sleep(2000);

            //获取需要滚动加载的次数
            var cardCount = int.Parse(driver.FindElements(By.CssSelector("_fd86t")).First().Text);
            var scrollCount = cardCount / 12 == 0 ? cardCount / 12 : (cardCount / 12) + 1;

            //默认滚动条移动到y轴3000的位置
            var initialC = 3000;
            var i = 0;
            do
            {
                initialC += 50;//每次递增滚动50的距离
                ((IJavaScriptExecutor)driver).ExecuteScript($"scrollTo(0,{initialC});");
                System.Threading.Thread.Sleep(1000);
                i++;
            } while (i < scrollCount);
            var defaultPath = Environment.CurrentDirectory + "\\" + insName;
            var filePath = $"{defaultPath}.html";
            SaveNetFile(defaultPath, url, filePath);
        }

        /// <summary>
        /// 解析出需要下载的资源数
        /// </summary>
        /// <param name="htmlFilePath">已经解析的html文件</param>
        public void ExtraDownloadSource(string htmlFilePath)
        {
            if (!File.Exists(htmlFilePath)) throw new IOException("文件不存在");
            //读取文件
            var reader = new StreamReader(htmlFilePath);
            var html = reader.ReadToEnd();

            //解析出资源
            var regExt = new Regex("\\<a\\s*(?<href>\\w+\\s*=\\s*\"\\/\\w+\\/.+\\?taken-by=\\w+\"\\>)");
            var matchs = regExt.Matches(html);

            if (matchs != null && matchs.Count > 0)
            {
                for (int i = 0; i < matchs.Count; i++)
                {
                    var url = matchs[i].Groups["media"].Value;
                    mediaService.AddDownload(new DownloadEntity
                    {
                        Disabled = 0,
                        DownloadId = Guid.NewGuid(),
                        HttpUrl = url,
                        Rec_CreateBy = sys,
                        Rec_CreateTime = DateTime.Now,
                    });
                }
            }
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        /// <returns></returns>
        private PhantomJSDriverService GetPhantomJSDriverService()
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
