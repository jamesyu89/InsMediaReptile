using System;
using System.Collections.Generic;
using InstagramPhotos.Utility.CommonQuery;

namespace InstagramPhotos.QueryModel.Sys
{
    /// <summary>
    /// LogInfoQO
    /// </summary>
    public class LogInfoQO : IQueryEntity
    {

        /// <summary>
        /// LogInfoQO 构造函数
        /// </summary>
        public LogInfoQO()
        { }


        private string tablename = "Fct_Sys_LogInfo";
        /// <summary>
        /// 表名
        /// </summary>
        public override string TableName
        {
            get { { return tablename; } }
            set { tablename = value; }
        }

        /// <summary>
        /// 查询主键名
        /// </summary>
        protected override string IdName
        {
            get { return "LogId"; }
        }

        public override List<QueryParamater> QueryPars { get; set; }

        #region 查询条件

        /// <summary>
        /// 通用查询条件
        /// </summary>
        public enum QueryEnums
        {
            /// <summary>
            /* 1: 日志Id*/
            /// </summary>
            LogId,
            /// <summary>
            /* 2: 应用类型 1 Web  2 Api 3 GaeaService*/
            /// </summary>
            AppType,
            /// <summary>
            /* 3: 应用名称*/
            /// </summary>
            AppName,
            /// <summary>
            /* 4: 程序集类名*/
            /// </summary>
            ClassFullName,
            /// <summary>
            /* 5: 方法名*/
            /// </summary>
            MethodName,
            /// <summary>
            /* 6: 机器名*/
            /// </summary>
            MachineName,
            /// <summary>
            /* 7: 当前进程名*/
            /// </summary>
            ProcessName,
            /// <summary>
            /* 8: 主机名称*/
            /// </summary>
            HoustName,
            /// <summary>
            /* 9: 主机IP*/
            /// </summary>
            HoustIp,
            /// <summary>
            /* 10: 客户端地址*/
            /// </summary>
            ClientAddress,
            /// <summary>
            /* 11: 标签*/
            /// </summary>
            Tag,
            /// <summary>
            /* 12: 操作人名*/
            /// </summary>
            OpName,
            /// <summary>
            /* 13: 日志级别*/
            /// </summary>
            Level,
            /// <summary>
            /* 14: 输入内容*/
            /// </summary>
            Input,
            /// <summary>
            /* 15: 输出内容*/
            /// </summary>
            Output,
            /// <summary>
            /* 16: */
            /// </summary>
            ResponseTime,
            /// <summary>
            /* 17: 消息提示*/
            /// </summary>
            Message,
            /// <summary>
            /* 18: 堆栈信息*/
            /// </summary>
            StackTrace,
            /// <summary>
            /* 19: 备注*/
            /// </summary>
            Remark,
            /// <summary>
            /* 20: 创建时间*/
            /// </summary>
            Rec_CreateTime,
            /// <summary>
            /* 21: 创建人名称*/
            /// </summary>
            Rec_CreateBy,
        }
        #endregion



    }
}
