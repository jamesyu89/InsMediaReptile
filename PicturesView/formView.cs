using InstagramPhotos.Utility.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicturesView
{
    public partial class formView : Form
    {
        private List<string> _imageList = new List<string>();
        /// <summary>
        /// 当前最近修改的目录时间
        /// </summary>
        private DateTime _currentDirLastTime;
        /// <summary>
        /// 当前最近修改的图片时间
        /// </summary>
        private DateTime _currentPicLastTime;
        /// <summary>
        /// 当前最早修改的目录时间
        /// </summary>
        private DateTime _currentDirEarilyTime;
        /// <summary>
        /// 当前最早修改的图片时间
        /// </summary>
        private DateTime _currentPicEarilyTime;
        /// <summary>
        /// 当前展示的目录
        /// </summary>
        private string _currentDirName;
        /// <summary>
        /// 当前是正序还是倒序
        /// </summary>
        private int _sort = int.Parse(AppSettings.GetValue<string>("Sort", "0"));
        /// <summary>
        /// 当前窗体控件集合
        /// </summary>
        public List<Control> _controlList = new List<Control>();

        public formView()
        {
            InitializeComponent();
        }

        private void formView_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
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
                        if (_sort == 0)//正序
                        {
                            var tieDir = dirs[i].GetDirectories().OrderBy(o => o.CreationTime).ToList();
                            _currentDirName = tieDir.FirstOrDefault().Name;
                            _currentPicEarilyTime = tieDir.FirstOrDefault().CreationTime;
                            var files = tieDir.FirstOrDefault().GetFiles().OrderBy(o => o.CreationTime);
                            listImage.AddRange(files.Select(f => f.FullName));
                        }
                        else//倒序
                        {
                            var tieDir = dirs[i].GetDirectories().OrderByDescending(o => o.LastAccessTime).ToList();
                            _currentDirName = tieDir.FirstOrDefault().Name;
                            _currentDirLastTime = tieDir.FirstOrDefault().LastAccessTime;
                            var files = tieDir.FirstOrDefault().GetFiles().OrderBy(o => o.LastAccessTime);
                            listImage.AddRange(files.Select(f => f.FullName));
                        }

                    }
                }
            }
            if (listImage.Any())
            {
                try
                {
                    GetControl(this.Controls);
                    var picBoxes = new List<PictureBox>();
                    var labels = new List<Label>();
                    foreach (Control ctrl in _controlList)
                    {
                        if (ctrl is PictureBox)
                            picBoxes.Add((PictureBox)ctrl);
                        if (ctrl is Label)
                            labels.Add((Label)ctrl);
                    }
                    if (picBoxes.Any())
                    {
                        for (int i = 0; i < picBoxes.Count; i++)
                        {
                            Action<string> setPic = (a) => { picBoxes[i].ImageLocation = a; };
                            Action<string> setLabel = (a) => { labels[i].Text = a; };
                            picBoxes[i].Invoke(setPic, listImage[i]);
                            labels[i].Invoke(setLabel, _currentDirName);
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void GetControl(Control.ControlCollection ctc)
        {
            foreach (Control ct in ctc)
            {
                _controlList.Add(ct);
                //C#只遍历窗体的子控件，不遍历孙控件
                //当窗体上的控件有子控件时，需要用递归的方法遍历，才能全部列出窗体上的控件
                if (ct.HasChildren)
                {
                    GetControl(ct.Controls);
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }
    }
}
