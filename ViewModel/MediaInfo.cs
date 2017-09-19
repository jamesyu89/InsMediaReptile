using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    /// <summary>
    /// 队列临时类  
    /// </summary>
    public class MediaInfo
    {
        /// <summary>
        /// Ins用户名
        /// </summary>
        public string InsName { get; set; }
        /// <summary>
        /// 资源文件网络路径
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 要保存到文件(html)的(包含路径的)全名
        /// </summary>
        public string FileFullName
        {
            get
            {
                return DefaultPath + $"\\{InsName}.html";
            }
        }
        /// <summary>
        /// 文件资源类型,逗号隔开
        /// </summary>
        public string MetaTypeList { get; set; }
        /// <summary>
        /// 匹配资源的正则表达式,逗号隔开
        /// </summary>
        public string RegexList { get; set; }
        /// <summary>
        /// 默认路径
        /// </summary>
        public string DefaultPath
        {
            get
            {
                return Environment.CurrentDirectory + "\\" + this.InsName;
            }
        }
    }
}
