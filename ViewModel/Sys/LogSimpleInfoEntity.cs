using System;

namespace InstagramPhotos.ViewModel.Sys
{
    /// <summary>
    /// LogInfoEntity
    /// </summary>
    [Serializable]
    public class LogSimpleInfoEntity
    {

        /// <summary>
        /// LogInfoEntity 构造函数
        /// </summary>
        public LogSimpleInfoEntity()
        { }


        #region Members


        #endregion

        /// <summary>
        /* 1: 日志Id*/
        /// </summary>
        public Guid LogId { get; set; }
        /// <summary>
        /* 2: 应用类型 1 Web  2 Api 3 GaeaService*/
        /// </summary>
        public int AppType { get; set; }
        /// <summary>
        /* 3: 应用名称*/
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /* 4: 程序集类名*/
        /// </summary>
        public string ClassFullName { get; set; }
        /// <summary>
        /* 5: 方法名*/
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /* 6: 机器名*/
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /* 7: 当前进程名*/
        /// </summary>
        public string ProcessName { get; set; }
        /// <summary>
        /* 8: 主机名称*/
        /// </summary>
        public string HoustName { get; set; }
        /// <summary>
        /* 9: 主机IP*/
        /// </summary>
        public string HoustIp { get; set; }
        /// <summary>
        /* 10: 客户端地址*/
        /// </summary>
        public string ClientAddress { get; set; }
        /// <summary>
        /* 11: 标签*/
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /* 12: 操作人名*/
        /// </summary>
        public string OpName { get; set; }
        /// <summary>
        /* 13: 日志级别*/
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /* 16: */
        /// </summary>
        public string ResponseTime { get; set; }
        /// <summary>
        /* 20: 创建时间*/
        /// </summary>
        public DateTime Rec_CreateTime { get; set; }
        /// <summary>
        /* 21: 创建人名称*/
        /// </summary>
        public string Rec_CreateBy { get; set; }


    }
}
