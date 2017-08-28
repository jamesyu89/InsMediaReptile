using System;

namespace InstagramPhotos.Utility.Model
{
    /// <summary>
    /// 业务层返回业务结果对象
    /// </summary>
    public class BizResult<T>
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isOK">操作业务结果</param>
        public BizResult(bool isOK)
        {
            IsOK = isOK;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isOK">操作业务结果</param>
        /// <param name="resultCode">业务结果代码</param>
        public BizResult(bool isOK, int resultCode)
        {
            IsOK = isOK;
            Code = resultCode;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isOK">操作业务结果</param>
        /// <param name="message">业务结果信息</param>
        public BizResult(bool isOK, string message)
        {
            IsOK = isOK;
            Message = message;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isOK">操作业务结果</param>
        /// <param name="resultCode">业务结果代码</param>
        /// <param name="message">业务结果信息</param>
        public BizResult(bool isOK, int resultCode, string message)
        {
            IsOK = isOK;
            Code = resultCode;
            Message = message;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 业务结果标志 (true:成功,false：失败)
        /// </summary>
        public bool IsOK { get; set; }

        /// <summary>
        /// 业务结果代码(如0成功，-1:失败，-4异常）
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 业务结果消息 (如操作成功/失败等）
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回对象
        /// </summary>
        public T ReturnObject { get; set; }

        /// <summary>
        /// 返回内部异常
        /// </summary>
        public Exception InnerException { get; set; }

        /// <summary>
        /// 返回的扩展属性
        /// </summary>
        public object ObjectExtention { get; set; }

        #endregion
    }
}
