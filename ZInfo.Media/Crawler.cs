using InstagramPhotos.Framework.Common;
using InstagramPhotos.Utility.Configuration;
using OpenQA.Selenium.PhantomJS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZInfo.Media
{
    public partial class Crawler : Form
    {

        #region Ctor. && Init

        private List<string> UrlList = new List<string>();
        private string Domain = AppSettings.GetValue<string>("Domain");
        private CancellationTokenSource AnalyCancelToken = new CancellationTokenSource();
        private int t = 0;
        /// <summary>
        /// 当前展示到界面上的几张美图
        /// </summary>
        private Queue<string> CurrentImageList = new Queue<string>();
        private List<string> _imageList = new List<string>();
        /// <summary>
        /// 初始化时默认为加载第1页的帖子
        /// </summary>
        private int CurrentPage = 1;
        public Crawler()
        {
            InitializeComponent();
        }

        private void Crawler_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;    //最大化窗体 

            //加载cc域名对应的可用地址
            Action<string> setTbx1 = (s) => { textBox1.Text = s; };
            Action<string> setTbx2 = (s) => { textBox2.Text = s; };
            Action<string> setTbx3 = (s) => { textBox3.Text = s; };
            Task.Run(() =>
            {
                var _service = PhantomJSDriverService.CreateDefaultService();
                _service.HideCommandPromptWindow = true;
                var driver = new PhantomJSDriver(_service);
                driver.Navigate().GoToUrl("http://zc.97down.info");
                var domain = driver.Url;
                var zp = domain + AppSettings.GetValue<string>("ListPageUrl_ZP");
                var wm = domain + AppSettings.GetValue<string>("ListPageUrl_WM");
                var lc = domain + AppSettings.GetValue<string>("ListPageUrl_LC");
                textBox1.Invoke(setTbx1, zp);
                textBox2.Invoke(setTbx2, wm);
                textBox3.Invoke(setTbx3, lc);
            });


        }

        private void GetUrls()
        {
            UrlList.Clear();
            if (checkBox1.Checked)
                UrlList.Add(textBox1.Text);
            if (checkBox2.Checked)
                UrlList.Add(textBox2.Text);
            if (checkBox3.Checked)
                UrlList.Add(textBox3.Text);
        }

        #endregion

        #region Events

        //取消
        private void button2_Click(object sender, EventArgs e)
        {
            AnalyCancelToken.Cancel();
        }

        private void Crawler_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //取消"关闭窗口"事件
                e.Cancel = true; // 取消关闭窗体 

                //使关闭时窗口向右下角缩小的效果
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.Visible = true;
                //this.m_cartoonForm.CartoonClose();
                this.Hide();
                return;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.Visible = true;
                this.Hide();
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void toolStripMenuItemMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.notifyIcon1.Visible = true;
            this.Hide();
        }

        private void toolStripMenuItemMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.notifyIcon1.Visible = true;
            this.Show();
        }

        private void toolStripMenuItemNormal_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = true;
            this.Show();
        }

        private void toolStripMenuItemQuit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                AnalyCancelToken.Cancel();
                this.notifyIcon1.Visible = false;
                this.Close();
                this.Dispose();
                Environment.Exit(Environment.ExitCode);
            }
        }

        private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.Visible = true;
                this.Hide();
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }

        //定时更新最新下载的图片
        private void timer1_Tick(object sender, EventArgs e)
        {
            Action<string> setPic1 = (a) => { pictureBox1.ImageLocation = a; };
            Action<string> setPic2 = (a) => { pictureBox2.ImageLocation = a; };
            Action<string> setPic3 = (a) => { pictureBox3.ImageLocation = a; };
            Action<string> setPic4 = (a) => { pictureBox4.ImageLocation = a; };
            Task.Factory.StartNew(() =>
            {
                //加载图片
                var listImage = new List<string>();
                var basePath = AppSettings.GetValue<string>("SaveDir");
                var baseDir = new DirectoryInfo(basePath);
                if (baseDir != null)
                {
                    //获取应用程序的版块目录集
                    var dirs = baseDir.GetDirectories();
                    if (dirs != null && dirs.Any())
                    {
                        //遍历版块目录
                        for (int i = 0; i < dirs.Length; i++)
                        {
                            //遍历版块下的每个帖子目录
                            var tieDir = dirs[i].GetDirectories().OrderByDescending(o => o.LastAccessTime).ToList();

                            //取出每个帖子里下载的图片(只取大图,未下载完成的不加入到队列)
                            if (t < tieDir.Count)
                            {
                                var dirFiles = tieDir[t].GetFiles().Where(f => f.Length > 50 * 1024);
                                if (dirFiles != null && dirFiles.Any())
                                {
                                    dirFiles.Select(f => f.FullName).ToList().ForEach(f =>
                                    {
                                        _imageList.Add(f);
                                    });
                                }
                            }
                        }
                    }
                }
                else
                {
                    Directory.CreateDirectory(basePath);
                }
                if (_imageList.Any())
                {
                    var list = _imageList.Skip(t).Take(4).ToList();
                    try
                    {
                        pictureBox1.Invoke(setPic1, list[0]);
                        pictureBox2.Invoke(setPic2, list[1]);
                        pictureBox3.Invoke(setPic3, list[2]);
                        pictureBox4.Invoke(setPic4, list[3]);
                    }
                    catch (Exception)
                    {

                    }
                }
                t += 1;
            });
        }

        #endregion

        #region Methods

        private void button1_Click(object sender, EventArgs e)
        {
            AnalyCancelToken = new CancellationTokenSource();
            DownloadPictures(AnalyCancelToken.Token);
        }

        private async void DownloadPictures(CancellationToken token)
        {
            await Task.Factory.StartNew(() =>
            {
                GetUrls();
                if (UrlList != null)
                {
                    Parallel.For(0, UrlList.Count, (i) =>
                    {
                        while (CurrentPage < 500)
                        {
                            var httpUrl = $"{UrlList[i]}&page={CurrentPage}";
                            Print($"开始加载第【{CurrentPage}】页资源：{httpUrl}");
                            var listHtml = string.Empty;
                            try
                            {
                            PageList:
                                listHtml = HttpFileManager.GetHttpUrlString(httpUrl);
                                if (string.IsNullOrEmpty(listHtml))
                                {
                                    Print("网络出现异常，30秒后重试".Log());
                                    Thread.Sleep(1000 * 30);//等待30秒再试下一个
                                    goto PageList;
                                }
                            }
                            catch (Exception ex)
                            {
                                Print($"加载{httpUrl}出现异常，跳至下一个帖子{ex.Message}".Log());
                                continue;
                            }
                            Print($"资源加载成功，使用正则匹配可下载的资源");
                            var reg = new Regex("\\<a href\\=\"(?<href>htm_data[/0-9-a-z]+\\.html)\"\\sid[^>]+\\>(?<text>[^>]+)\\</a\\>");
                            var listMatchs = reg.Matches(listHtml);

                            if (listMatchs != null && listMatchs.Count > 0)
                            {
                                var _basePath = string.Empty;
                                Print($"匹配成功，当前匹配的是第{CurrentPage}页内容");
                                if (UrlList[i].Contains("fid=15"))
                                {
                                    _basePath = AppSettings.GetValue<string>("SaveDir") + "\\自拍偷拍";
                                }
                                else if (UrlList[i].Contains("fid=14"))
                                {
                                    _basePath = AppSettings.GetValue<string>("SaveDir") + "\\唯美写真";
                                }
                                else if (UrlList[i].Contains("fid=16"))
                                {
                                    _basePath = AppSettings.GetValue<string>("SaveDir") + "\\露出激情";
                                }
                                for (int j = 0; j < listMatchs.Count; j++)
                                {
                                    var dir = _basePath + "\\" + listMatchs[j].Groups["text"].Value;
                                    var htmlDetailUrl = Domain + "/" + listMatchs[j].Groups["href"].Value;
                                    Print($"检查目录{dir}是否存在");
                                    if (!Directory.Exists(dir))
                                    {
                                        Print($"目录不存在，正在创建目录");
                                        Directory.CreateDirectory(dir);
                                    }

                                    Print($"目录已创建，正在浏览当前页第{j + 1}个帖子");

                                    var detailHtml = HttpFileManager.GetHttpUrlString(htmlDetailUrl);
                                    if (!string.IsNullOrEmpty(detailHtml))
                                    {
                                        var detailReg = new Regex("(?<img>http:[^'\"]+\\.(jpg|jpeg|png|gif))");
                                        Print($"正在匹配链接加载成功后的可下载资源");
                                        var matchs = detailReg.Matches(detailHtml);
                                        //去重
                                        if (matchs != null && matchs.Count > 0)
                                        {
                                            var matchsResult = matchs.Cast<Match>().GroupBy(g => g.Groups["img"].Value).Distinct().ToList();
                                            Print($"匹配成功，共{matchsResult.Count()}条资源可下载");
                                            if (Directory.Exists(dir) && Directory.GetFiles(dir).Count() >= matchsResult.Count())
                                            {
                                                Print("此帖图片已全部下载，下一个");
                                                continue;
                                            }
                                            for (int x = 0; x < matchsResult.Count; x++)
                                            {
                                                var fileUrl = matchsResult[x].Key;
                                                var fileName = fileUrl.Substring(fileUrl.LastIndexOf('/') + 1);
                                                var filePath = dir + "\\" + fileName;
                                                Print($"[{x}]正在将文件{fileName}存放至{dir}");
                                                if (File.Exists(filePath))//文件存在，Pass
                                                {
                                                    //文件上次未下载成功，重新下载
                                                    if (new FileInfo(filePath).Length >= 35 * 1024)
                                                    {
                                                        Print($"文件[{fileName}]已存在，下一个...");
                                                        continue;
                                                    }
                                                }
                                                try
                                                {
                                                    Task.Factory.StartNew(() =>
                                                    {
                                                        HttpFileManager.DownloadFile(fileUrl, filePath);
                                                    });
                                                }
                                                catch (Exception e)
                                                {
                                                    Print("下载失败，" + e.Message + ",继续下一个图片的下载".Log());
                                                    continue;
                                                }
                                                Print($"文件下载成功");
                                                //Task.Run(() =>
                                                //{
                                                //    ShowPicture(dir + "\\" + fileName);
                                                //});
                                            }
                                        }
                                    }
                                    Print($"当前帖子图片已全部下载完毕");
                                }
                            }
                            CurrentPage += 1;
                        }
                    });
                }
                else
                {
                    MessageBox.Show("请选择一个列表页");
                    return;
                }
            }, token);
        }

        /// <summary>
        /// 打印消息到界面上
        /// </summary>
        /// <param name="message"></param>
        public void Print(string message)
        {
            Action<string> printMessage = (s) => richTextBox1.AppendText(s + Environment.NewLine);
            richTextBox1.Invoke(printMessage, message);
        }

        /// <summary>
        /// 图片显示到右边
        /// </summary>
        /// <param name="message"></param>
        public void ShowPicture(string picPath)
        {
            Action<string> setImg = (s) => pictureBox1.ImageLocation = picPath;
            pictureBox1.Invoke(setImg, picPath);
        }



        #endregion


    }
}
