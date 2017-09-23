using InstagramPhotos.Framework.Common;
using InstagramPhotos.Media.QueryModel;
using InstagramPhotos.QueryModel;
using InstagramPhotos.Utility.KVStore;
using InstagramPhotos.ViewModel;
using Service.BLL;
using Service.Interface.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ZInfo.Media
{
    public partial class Form1 : Form
    {
        #region Ctor. && Init

        static readonly IMediaService mediaService = ServiceFactory.GetInstance<IMediaService>();
        private readonly static Guid sys = Guid.Parse("3102A7AC-35DF-4C9C-8A11-CE9501EBE300");
        private CancellationTokenSource AnalyCancelToken = new CancellationTokenSource();
        private CancellationTokenSource DownloadCancelToken = new CancellationTokenSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        #endregion

        #region Events

        //分析
        private async void button1_Click(object sender, EventArgs e)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                while (!AnalyCancelToken.IsCancellationRequested)
                {
                    if (MediaQueueHelper.Instance.ListCount == 0)
                    {
                        Print("正在初始化...".Log());
                        Print("检查需要被执行的任务...".Log());
                        var mediaTaskQo = new MediaTaskQO();
                        mediaTaskQo.Equal(MediaTaskQO.QueryEnums.Disabled, 0);
                        var mediaResult = mediaService.GetMediataskDtosByPara(mediaTaskQo, false);
                        var taskRemainCount = mediaResult != null ? mediaResult.Count : 0;
                        Print($"当前任务数为{taskRemainCount}...".Log());
                        if (taskRemainCount > 0)
                        {
                            Print("正在将任务添加至队列中....".Log());
                            mediaResult.ForEach(m =>
                            {
                                MediaQueueHelper.Instance.AddQueue(m.Url, m.MediaTaskId);
                            });
                            Print("添加完成....".Log());
                        }
                        Print("初始化结束...".Log());
                        Print("处理队列中....".Log());
                        MediaQueueHelper.Instance.Start();
                        Console.WriteLine(Environment.NewLine);
                        Console.WriteLine(Environment.NewLine);
                    }
                    else
                    {
                        if (DefineMessageQueue.ItemQueue.Count > 0)
                        {
                            var message = DefineMessageQueue.OutFromQueue();
                            Print(message + Environment.NewLine);
                        }
                    }
                }
            }, AnalyCancelToken.Token);
        }

        //取消分析
        private void button3_Click(object sender, EventArgs e)
        {
            AnalyCancelToken.Cancel();
        }

        //下载
        private async void button2_Click(object sender, EventArgs e)
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                while (!DownloadCancelToken.IsCancellationRequested)
                {
                    #region 获取数据库已上传的下载资源地址

                    List<DownloadEntity> downloadTasks = null;
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
                                    Print("当前批次下载任务已完成，等待10秒继续扫描待下载的任务...".Log(), false);
                                    Thread.Sleep(10000);
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
                                Print("当前程序已重试4次，未正常获取到数据，程序正在退出...".Log(), false);
                                Environment.Exit(-1);//退出
                            }
                        }
                        catch (Exception ex)
                        {
                            Print($"查询下载任务出现异常，异常信息:{ex.Message}，正在进行第{i + 1}次重试...".Log(), false);
                            queryFlag -= 1;
                        }
                    }

                    #endregion

                    #region 执行下载任务，并保存到指定目录

                    for (int i = 0; i < downloadTasks.Count; i++)
                    {
                        if (!DownloadCancelToken.IsCancellationRequested)
                        {
                            try
                            {
                                Print($"[正在下载资源：{downloadTasks[i].HttpUrl}][{DateTime.Now}]".Log(), false);
                                // 设置参数
                                var httpUrl = downloadTasks[i].HttpUrl;
                                Stream rps = null;
                                try
                                {
                                    HttpWebRequest rq = WebRequest.Create(httpUrl) as HttpWebRequest;
                                    HttpWebResponse rp = rq.GetResponse() as HttpWebResponse;
                                    rps = rp.GetResponseStream();
                                }
                                catch (Exception ex)
                                {
                                    Print($"下载出现异常：{ex.Message}".Log(), false);
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
                                    Print($"[{sourceFileName}][此资源已下载，跳过]!".Log(), false);
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
                                Print($"[{sourceFileName}][资源下载完成！]".Log(), false);
                                downloadTasks[i].Disabled = 1;
                                downloadTasks[i].Rec_ModifyBy = sys;
                                downloadTasks[i].Rec_ModifyTime = DateTime.Now;
                                mediaService.UpdateDownload(downloadTasks[i]);
                            }
                            catch (Exception ex)
                            {
                                ex.Message.Log();
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    Print($"当前批次下载任务已完成，等待10秒继续扫描待下载的任务...[{DateTime.Now}]".Log(), false);
                    Thread.Sleep(10000);

                    #endregion
                }
            }, DownloadCancelToken.Token);
        }

        //取消下载
        private void button4_Click(object sender, EventArgs e)
        {
            DownloadCancelToken.Cancel();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.ScrollToCaret();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            richTextBox2.ScrollToCaret();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
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

        #endregion

        #region Methods

        /// <summary>
        /// 打印消息到界面上
        /// </summary>
        /// <param name="message"></param>
        public void Print(string message, bool showLeft = true)
        {
            if (showLeft)
            {
                Action<string> printMessage = (s) => richTextBox1.AppendText(s + Environment.NewLine);
                richTextBox1.Invoke(printMessage, message);
            }
            else
            {
                Action<string> printMessage = (s) => richTextBox2.AppendText(s + Environment.NewLine);
                richTextBox2.Invoke(printMessage, message);
            }
        }

        

        #endregion
    }
}
