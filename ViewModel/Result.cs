using System.Collections.Generic;

namespace InstagramPhotos.ViewModel
{
    /// <summary>
    /// 状态结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public object Data { get; set; }
    }

    /// <summary>
    /// 实体结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> where T : class
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 请求失败返回的消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否请求成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 请求成功返回的数据
        /// </summary>
        public T Data { get; set; }

        public int TotalCount { get; set; }

    }

    /// <summary>
    /// 列表项
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class ListResult<T> where T : class
    {

        public List<T> Datas { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 请求失败返回的消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否请求成功
        /// </summary>
        public bool IsSuccess { get; set; }
    }

    /// <summary>
    /// 分页列表项
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T> where T : class
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int TotalCount { get; set; }

        public List<T> Datas { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 请求失败返回的消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否请求成功
        /// </summary>
        public bool IsSuccess { get; set; }
    }


}