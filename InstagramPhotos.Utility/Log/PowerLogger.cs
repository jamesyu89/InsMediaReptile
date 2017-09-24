using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace InstagramPhotos.Utility.Log
{ /// <summary>
  /// 记录日志 本地测试查看日志使用 仅debug模式起作用 且通常写入同一固定目录下
  /// </summary>
    public class Power//Logger
    {
        #region 字段

        private static readonly object ObjSync = new object();

        /// <summary>
        /// 默认一个文本文件的最大值为3M
        /// </summary>
        private static readonly long MaxFileLength = 1024 * 1024 * 3;

        #endregion

        #region 方法

        private static void Add(string msg)
        {

            //return; //发布时取消注释

#if DEBUG

            bool isLocked = false;
            try
            {
                //根据配置判断是否需要写入本地文本日志
                var isWriteFileLog = ConfigurationManager.AppSettings["IsWriteFileLog"];
                if (isWriteFileLog != "1")
                {
                    return;
                }

                var strDateTime = DateTime.Now.ToString("yyyyMMdd");

                isLocked = Monitor.TryEnter(ObjSync, 100);
                if (isLocked == false)
                {
                    return;
                }

                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        string.Format("LOG_{0}", DateTime.Now.ToString("MM")));  //保存日志所在路径
                var isWeb = CheckIsWebApp(); //是否web应用
                if (isWeb == true)
                {
                    logPath = string.Format(@"C:\Power//Logger\LOG_{0}", DateTime.Now.ToString("MM"));
                }

                if (Directory.Exists(logPath) == false)
                {
                    Directory.CreateDirectory(logPath);
                }

                logPath = Path.Combine(logPath, strDateTime);
                if (Directory.Exists(logPath) == false)
                {
                    Directory.CreateDirectory(logPath);
                }

                var stackFrame = new StackFrame(2, false);
                logPath = Path.Combine(logPath, stackFrame.GetMethod().DeclaringType.FullName);
                if (Directory.Exists(logPath) == false)
                {
                    Directory.CreateDirectory(logPath);
                }

                var currentIndex = 0;
                currentIndex = Directory.GetFiles(logPath)
                    .Where(x => x.Contains(".log") && x.Contains("_") && new FileInfo(x).Name.Contains(strDateTime))
                    .Select(x =>
                    {
                        var index = -1;
                        int.TryParse(new FileInfo(x).Name.Split('_')[1].Replace(".log", string.Empty), out index);
                        return Math.Abs(index);
                    })
                    .OrderByDescending(x => x).FirstOrDefault();

                string filePath = Path.Combine(logPath, string.Format("{0}_{1}.log", strDateTime, currentIndex));

                if (File.Exists(filePath) == true)
                {
                    var fileInfo = new FileInfo(filePath);
                    var len = fileInfo.Length;
                    //单个文件超过最大值 新建一个文本文件
                    if (len > MaxFileLength)
                    {
                        filePath = Path.Combine(logPath, string.Format("{0}_{1}", strDateTime, currentIndex + 1) + ".log");
                    }
                }

                File.AppendAllText(filePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), System.Text.Encoding.UTF8);
                File.AppendAllText(filePath, Environment.NewLine);
                File.AppendAllText(filePath, msg, System.Text.Encoding.UTF8);
                File.AppendAllText(filePath, Environment.NewLine, System.Text.Encoding.UTF8);
                File.AppendAllText(filePath, Environment.NewLine, System.Text.Encoding.UTF8);
            }
            catch
            {
            }
            finally
            {
                if (isLocked)
                {
                    Monitor.Exit(ObjSync);
                }
            }


#endif
        }

        public static void Add(Exception ex)
        {
            if (ex == null)
            {
                return;
            }
            Add(ex.ToString());
        }

        public static void Add(string msg, params object[] objArr)
        {
            try
            {
                if (objArr == null || objArr.Length == 0)
                {
                    Add(msg);
                    return;
                }

                string strMsg = string.Format(msg, objArr);
                Add(strMsg);
            }
            catch
            {
            }
        }

        public static void AddESBLog<TRequest, TResponse>(string msg, TRequest request, TResponse response)
        {
            try
            {
                var strRequest = Serialize(request);
                var strResponse = Serialize(response);
                var realMsg = string.Format("{0}{1}{1}请求：{2}{3}响应：{4}", msg, Environment.NewLine, strRequest, Environment.NewLine, strResponse);
                Add(realMsg);
            }
            catch
            {
            }
        }

        #endregion

        #region 帮助方法

        /// <summary>
        /// 对象转换成json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonObject">需要格式化的对象</param>
        /// <returns>Json字符串</returns>
        private static string Serialize<T>(T jsonObject)
        {
            string json = null;
            try
            {
                if (jsonObject == null)
                {
                    return json;
                }
                var serializer = new DataContractJsonSerializer(typeof(T));

                using (var ms = new MemoryStream()) //定义一个stream用来存发序列化之后的内容
                {
                    serializer.WriteObject(ms, jsonObject);
                    json = Encoding.UTF8.GetString(ms.GetBuffer()); //将stream读取成一个字符串形式的数据，并且返回
                    ms.Close();
                }
            }
            catch
            {
                json = string.Empty;
            }
            return json;
        }

        /// <summary>
        /// json字符串转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">要转换成对象的json字符串</param>
        /// <returns></returns>
        private static T Deserialize<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            T obj = default(T);
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                obj = (T)serializer.ReadObject(ms);
                ms.Close();
            }
            return obj;
        }

        /// <summary>
        /// 判断当前应用是否为web
        /// </summary>
        /// <returns></returns>
        private static bool CheckIsWebApp()
        {
            var isWeb = false;
            var hsWebProcess = new HashSet<string>
            {
                "w3wp",
                "aspnet_wp",
                "webdev.webserver"
            };

            try
            {
                var processName = Process.GetCurrentProcess().ProcessName.ToLower();
                if (hsWebProcess.Contains(processName) == true)
                {
                    isWeb = true;
                }
                else if (System.Web.HttpContext.Current != null)
                {
                    isWeb = true;
                }
            }
            catch
            {
                isWeb = false;
            }

            return isWeb;
        }

        #endregion

    }
}