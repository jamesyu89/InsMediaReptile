using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jns.BasicService.Log.Info;
using Jns.BasicService.Log.Storage;
using ServiceStack;
using InstagramPhotos.Utility.Configuration;

namespace InstagramPhotos.Utility.Log
{
    public class JnsLog
    {
        public static LogStorageServer storageServer = new LogStorageServer();
        public static LogStorageDisk storageDisk = new LogStorageDisk();
        public static LogStorageDisk storageDiskCustom = new LogStorageDisk("QMS.log");
        public static string appName = AppSettings.GetValue<string>("Gaea.AppName", "InstagramPhotos.ServerAgent");

        public static void Error(string msg, Jns.BasicService.Log.LogLevel level = Jns.BasicService.Log.LogLevel.Error)
        {
            var appInfo = new LogAppInfo
            {
                Message = msg,
                Level = level,
                AppName = appName
            };
            storageServer.Save(appInfo);
        }
        public static void Error(Exception ex, string msg = "")
        {
            if (string.IsNullOrEmpty(msg))
            {
                msg = ex.Message;
            }
            else
            {
                msg = string.Format("{0}:{1}", msg, ex.Message);
            }
            Error(msg);
        }

        public static void Exception(Exception ex, string msg = null)
        {
            var appInfo = new MyCustomLog()
            {
                ClassFullName = ex.Source,
                MethodName = ex.TargetSite?.Name ?? string.Empty,
                Message = msg??ex.StackTrace,
                Level = Jns.BasicService.Log.LogLevel.Error,
                AppName = appName,

            };
            storageServer.Save(appInfo);
        }

        public static void Exception(string className, string methodName, string input, string output, string msg)
        {
            var appInfo = new MyCustomLog
            {
                ClassFullName = className,
                MethodName = methodName,
                Input = input,
                Output = output,
                Message = msg,
                Level = Jns.BasicService.Log.LogLevel.Error,
                AppName = appName
            };
            storageServer.Save(appInfo);
        }

        public static void Info(string msg)
        {
            var appInfo = new LogAppInfo
            {
                OpName = "",
                Message = msg,
                Level = Jns.BasicService.Log.LogLevel.Info,
                AppName = appName
            };
            storageServer.Save(appInfo);
        }
        public static void Warn(string msg)
        {
            var appInfo = new LogAppInfo
            {
                OpName = "",
                Message = msg,
                Level = Jns.BasicService.Log.LogLevel.Warn,
                AppName = appName
            };
            storageServer.Save(appInfo);
        }

        public static void Debug(string msg)
        {
            var appInfo = new LogAppInfo
            {
                OpName = "",
                Message = msg,
                Level = Jns.BasicService.Log.LogLevel.Debug,
                AppName = appName
            };
            storageServer.Save(appInfo);
        }

        public static void Trace(string requestUri, string requestMethod, string input,string output,string msg)
        {
            var appInfo = new CustomLog
            {
                RequestUri = requestUri,
                MethodName = requestMethod,
                Input = input,
                Output = output,
                Message = msg,
                Level = Jns.BasicService.Log.LogLevel.Info,
                AppName = appName
            };
            storageServer.Save(appInfo);
        }

        public static void Info(string classFullName, string methodName,string message, string input="", string output="")
        {
            var appInfo = new LogAppInfo
            {
                ClassFullName = classFullName,
                MethodName = methodName,
                Message = message,
                Level = Jns.BasicService.Log.LogLevel.Info,
                AppName = appName
            };
            storageServer.Save(appInfo);
        }

        
    }

    /// <summary>
    /// 基于日志基类，用户自定义类结构
    /// </summary>
    public class CustomLog : LogBaseInfo
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUri { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 输入
        /// </summary>
        public string Input { get; set; }
        /// <summary>
        /// 输出
        /// </summary>
        public string Output { get; set; }
    }


    public class MyCustomLog : LogBaseInfo
    {
        /// <summary>
        /// 类全名（包括命名空间）
        /// 
        /// </summary>
        public string ClassFullName { get; set; }
        /// <summary>
        /// 方法名
        /// 
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// 操作用户
        /// 
        /// </summary>
        public string OpName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Input { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Output { get; set; }

    }
}
