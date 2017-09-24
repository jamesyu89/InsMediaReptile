using InstagramPhotos.Framework.Common;
using InstagramPhotos.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        private string BasePath = AppSettings.GetValue<string>("SaveDir");
        private CancellationTokenSource AnalyCancelToken = new CancellationTokenSource();
        public Crawler()
        {
            InitializeComponent();
        }

        private void Crawler_Load(object sender, EventArgs e)
        {
            var domain = AppSettings.GetValue<string>("Domain");
            var zp = domain + AppSettings.GetValue<string>("ListPageUrl_ZP");
            var wm = domain + AppSettings.GetValue<string>("ListPageUrl_WM");
            var lc = domain + AppSettings.GetValue<string>("ListPageUrl_LC");
            textBox1.Text = zp;
            textBox2.Text = wm;
            textBox3.Text = lc;
        }

        private void GetUrls()
        {
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

        #endregion


        #region Methods

        private async void button1_Click(object sender, EventArgs e)
        {
            AnalyCancelToken = new CancellationTokenSource();
            await Task.Run(() =>
            {
                DownloadPictures();
            }, AnalyCancelToken.Token);
        }

        private void DownloadPictures()
        {
            GetUrls();
            if (UrlList != null)
            {
                for (int i = 0; i < UrlList.Count; i++)//指定列表页
                {
                    var pageIndex = 1;
                    while (pageIndex < 500)
                    {
                        var httpUrl = $"{UrlList[i]}&page={pageIndex}";
                        Print($"开始加载第【{pageIndex}】页资源：{httpUrl}");
                        var listHtml = string.Empty;
                        try
                        {
                        PageList:
                            listHtml = HttpFileManager.GetHttpUrlString(httpUrl);
                            if (string.IsNullOrEmpty(listHtml))
                            {
                                Print("网络出现异常，30秒后重试");
                                Thread.Sleep(1000 * 30);//等待30秒再试下一个
                                goto PageList;
                            }
                        }
                        catch (Exception ex)
                        {
                            Print($"加载{httpUrl}出现异常，跳至下一个帖子" + ex.Message);
                            continue;
                        }
                        Print($"资源加载成功，使用正则匹配可下载的资源");
                        var reg = new Regex("\\<a href\\=\"(?<href>htm_data[/0-9-a-z]+\\.html)\"\\sid[^>]+\\>(?<text>[^>]+)\\</a\\>");
                        var listMatchs = reg.Matches(listHtml);

                        if (listMatchs != null && listMatchs.Count > 0)
                        {
                            Print($"匹配成功，当前匹配的是第{pageIndex}页内容");
                            for (int j = 0; j < listMatchs.Count; j++)
                            {
                                var dir = BasePath + "\\" + listMatchs[j].Groups["text"].Value;
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
                                                continue;
                                            try
                                            {
                                                HttpFileManager.DownloadFile(fileUrl, filePath);
                                            }
                                            catch (Exception e)
                                            {
                                                Print("下载失败，" + e.Message + ",继续下一个图片的下载");
                                                continue;
                                            }
                                            Print($"文件下载成功");
                                            Task.Run(() =>
                                            {
                                                ShowPicture(dir + "\\" + fileName);
                                            });
                                        }
                                    }
                                }
                                Print($"当前帖子图片已全部下载完毕");
                            }
                        }
                        pageIndex += 1;
                    }
                }
            }
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
