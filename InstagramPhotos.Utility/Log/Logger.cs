using System;
using System.Collections;
using System.Reflection;
using log4net;
using log4net.Config;
using YGOP.ESB.Log.Model;

namespace InstagramPhotos.Utility.Log
{
    public class Logger
    {
        public static Func<ICollection> Config = XmlConfigurator.Configure;

        public static ILog Instance = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static event EventHandler<LogEventArgs> OnLogged;

        public static void Info(string message)
        {
            Instance.Info(message);
            if (OnLogged != null)
                OnLogged(null, new LogEventArgs(message, LogLevel.Info));
        }

        public static void Info(string msgFormat, params object[] args)
        {
            string msg = string.Format(msgFormat, args);
            Info(msg);
        }

        public static void Warning(string message)
        {
            Instance.Warn(message);
            if (OnLogged != null)
                OnLogged(null, new LogEventArgs(message, LogLevel.Warn));
        }

        public static void Warning(string msgFormat, params object[] args)
        {
            string msg = string.Format(msgFormat, args);
            Warning(msg);
        }

        public static void Error(string message)
        {
            message = string.Format("【QMS异常】{0}", message);
            Instance.Error(message);

            JnsLog.Error(message);
            if (OnLogged != null)
                OnLogged(null, new LogEventArgs(message, LogLevel.Error));
        }

        public static void Error(string msgFormat, params object[] args)
        {
            string msg = string.Format(msgFormat, args);
            Error(msg);
        }

        public static void Exception(string message, Exception exception)
        {
           //YGOP.ESB.Log.LogManager.WriteFatal(message,exception);
            Instance.Error(message, exception);
            JnsLog.Exception(exception,message);
            if (OnLogged != null)
                OnLogged(null, new LogEventArgs(message, LogLevel.Warn, exception));
        }

        public static void Exception(Exception exception, string msgFormat, params object[] args)
        {
            string msg = string.Format(msgFormat, args);
            Exception(string.Format("【QMS异常】{0}", msg), exception);
        }

        /// <summary>
        ///     Log Exception
        /// </summary>
        /// <remarks>overload by Wayne 2012-12-09</remarks>
        /// <param name="exception">exception to log.</param>
        public static void Exception(Exception exception)
        {
            Instance.Error(string.Format("【QMS异常】{0}", exception.Message), exception);
            JnsLog.Exception(exception);
        }
    }

    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(string message, LogLevel level)
        {
            Message = message;
            Level = level;
        }

        public LogEventArgs(string message, LogLevel level, Exception ex)
        {
            Message = message;
            Level = level;
            Exception = ex;
        }

        public string Message { get; set; }
        public LogLevel Level { get; set; }
        public Exception Exception { get; set; }
    }

    public enum LogLevel : uint
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}