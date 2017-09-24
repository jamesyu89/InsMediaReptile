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

        private async void button1_Click(object sender, EventArgs e)
        {
            AnalyCancelToken = new CancellationTokenSource();
            await Task.Run(() =>
                {
                    GetUrls();
                    if (UrlList != null)
                    {
                        for (int i = 0; i < UrlList.Count; i++)//指定列表页
                        {
                            var pageIndex = 1;
                            while (pageIndex < 500)
                            {
                                Print($"开始加载资源：{UrlList[i]}");
                                var httpUrl = $"{UrlList[i]}&page={pageIndex}";
                                var listHtml = HttpFileManager.GetHttpUrlString(httpUrl);
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

                                        Print($"目录已创建，正在浏览当前页第一个链接");

                                        var detailHtml = HttpFileManager.GetHttpUrlString(htmlDetailUrl);
                                        if (!string.IsNullOrEmpty(detailHtml))
                                        {
                                            var detailReg = new Regex("(?<img>http:[^'\"]+\\.(jpg|jpeg|png|gif))");
                                            Print($"正在匹配链接加载成功后的可下载资源");
                                            var matchs = detailReg.Matches(detailHtml);
                                            //去重
                                            if (matchs != null && matchs.Count > 0)
                                            {
                                                var matchsResult = matchs.Cast<Match>().GroupBy(g => g.Groups["img"].Value).Distinct();
                                                Print($"匹配成功，共{matchsResult.Count()}条资源可下载");
                                                if (Directory.Exists(dir) && Directory.GetFiles(dir).Count() >= matchsResult.Count())
                                                {
                                                    Print("此帖图片已全部下载，下一个");
                                                    continue;
                                                }
                                                for (int x = 0; x < matchs.Count; x++)
                                                {
                                                    var fileUrl = matchs[x].Groups["img"].Value;
                                                    var fileName = fileUrl.Substring(fileUrl.LastIndexOf('/') + 1);
                                                    var filePath = dir + "\\" + fileName;
                                                    Print($"[{x}]正在将文件{fileName}存放至{dir}");
                                                    if (File.Exists(filePath))//文件存在，Pass
                                                        continue;
                                                    HttpFileManager.DownloadFile(fileUrl, filePath);
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
                }, AnalyCancelToken.Token);
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
    }
}
