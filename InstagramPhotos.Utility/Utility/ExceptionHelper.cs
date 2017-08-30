using System;
using System.Diagnostics;
using InstagramPhotos.Utility.Log;

namespace InstagramPhotos.Utility.Utility
{
    public sealed class ExceptionHelper
    {
        [DebuggerStepThrough]
        public static void LogException(Exception ex)
        {
            LogException(ex.Message, ex);
        }

        [DebuggerStepThrough]
        public static void LogException(Exception ex, string key)
        {
            LogException(ex.Message, ex, true, key);
        }

        [DebuggerStepThrough]
        public static void LogException(string message)
        {
            LogException(message, null, true, string.Empty, false);
        }

        [DebuggerStepThrough]
        public static void LogException(string message, bool isLog)
        {
            LogException(message, null, true, string.Empty, isLog);
        }

        [DebuggerStepThrough]
        public static void LogException(string message, string key)
        {
            LogException(message, null, true, key);
        }

        [DebuggerStepThrough]
        public static void LogException(string message, Exception ex)
        {
            LogException(message, ex, true);
        }

        [DebuggerStepThrough]
        public static void LogException(string message, Exception ex, bool isThrow)
        {
            LogException(message, ex, isThrow, string.Empty);
        }

        [DebuggerStepThrough]
        public static void LogException(string message, Exception ex, bool isThrow, string key)
        {
            LogException(message, ex, isThrow, key, true);
        }

        [DebuggerStepThrough]
        public static void LogException(string message, Exception ex, bool isThrow, string key, bool isLog)
        {
            if (string.IsNullOrEmpty(key))
            {
                if (isLog)
                    Logger.Exception(ex, message + " Key:{0}", key);
                if (isThrow)
                    throw new BusinessException(message, ex);
            }
            else
            {
                if (isLog)
                    Logger.Exception(message, ex);
                if (isThrow)
                    throw new BusinessException(message, ex, key);
            }
        }


        //[Obsolete]
        //public static void LogException(Exception ex, ErrorCode errorCode)
        //{

        //}

        //[Obsolete]
        //public static void LogException(Exception ex, ErrorCode errorCode, Module moduleName)
        //{

        //}

        //[Obsolete]
        //public static void LogException(Exception ex, ErrorCode errorCode, Module moduleName, bool isThrow)
        //{

        //}
    }

    public enum ErrorCode
    {

    }

    public enum Module
    {

    }

    [Serializable]
    public class BusinessException : Exception
    {
        public object Tag;

        public string EntityKey { get; set; }

        public BusinessException() { }
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, Exception inner) : base(message, inner) { }
        public BusinessException(string message, Exception inner, string entityKey)
            : base(message, inner)
        {
            EntityKey = entityKey;
        }
        protected BusinessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
