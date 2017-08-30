using System.Runtime.InteropServices;
using System.Text;

namespace InstagramPhotos.Utility.Configuration
{
    /// <summary>
    ///     ini配置文件操作类
    ///     <example>
    ///         [Email]
    ///         SendProperty = 0
    ///         Title = 测试邮箱标题 - 2006/06/13
    ///         Notice = 测试内容测试内容测试内容测试内容测试内容测试内容测试内容测试内容测试内容……
    ///         SMTPServer =
    ///         SMTPPort = 25
    ///         MailBox =
    ///         则操作实例代码：
    ///         <code>
    ///  IniHelper ini=new IniHelper (Server.MapPath(".")+"Email.ini");
    ///  ini.IniWriteValue("Email","Title","标题内容");
    ///  ini.IniWriteValue("Email","Notice","内容....");
    /// </code>
    ///     </example>
    /// </summary>
    public class IniHelper
    {
        /// <summary>
        ///     文件INI名称
        /// </summary>
        private readonly string _Path;

        /// <summary>
        ///     类的构造函数，传递INI文件名
        /// </summary>
        /// <param name="inipath"></param>
        public IniHelper(string inipath)
        {
            _Path = inipath;
        }

        /// <summary>
        ///     声明读写INI文件的API函数
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
            int size, string filePath);

        /// <summary>
        ///     写INI文件
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, _Path);
        }

        /// <summary>
        ///     读取INI文件指定
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            var temp = new StringBuilder(2048);
            GetPrivateProfileString(Section, Key, "", temp, 2048, _Path);
            return temp.ToString();
        }
    }
}