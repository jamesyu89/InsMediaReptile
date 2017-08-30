using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YGPS.QMS.DomainModel.Sys
{
    public class LogSimpleInfoDO
    {
        /// <summary>
        /// LogInfoDO 构造函数
        /// </summary>
        public LogSimpleInfoDO()
        { }


        /// <summary>
        /// 表名
        /// </summary>
        public static string TableName
        {
            get { return "Fct_Sys_LogInfo"; }
        }

        /// <summary>
        /// 查询主键名
        /// </summary>
        public static string IdName
        {
            get { return "LogId"; }
        }

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
        /// 
        /// </summary>
        public string Message { get; set; }
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
        #region 列名

        /// <summary>
        /// 列名枚举
        /// </summary>
        public enum ColumnEnum
        {
            /// <summary>
            /* 22: 日志Id*/
            /// </summary>
            LogId,
            /// <summary>
            /* 23: 应用类型 1 Web  2 Api 3 GaeaService*/
            /// </summary>
            AppType,
            /// <summary>
            /* 24: 应用名称*/
            /// </summary>
            AppName,
            /// <summary>
            /* 25: 程序集类名*/
            /// </summary>
            ClassFullName,
            /// <summary>
            /* 26: 方法名*/
            /// </summary>
            MethodName,
            /// <summary>
            /* 27: 机器名*/
            /// </summary>
            MachineName,
            /// <summary>
            /* 28: 当前进程名*/
            /// </summary>
            ProcessName,
            /// <summary>
            /* 29: 主机名称*/
            /// </summary>
            HoustName,
            /// <summary>
            /* 30: 主机IP*/
            /// </summary>
            HoustIp,
            /// <summary>
            /* 31: 客户端地址*/
            /// </summary>
            ClientAddress,
            /// <summary>
            /* 32: 标签*/
            /// </summary>
            Tag,
            /// <summary>
            /* 33: 操作人名*/
            /// </summary>
            OpName,
            /// <summary>
            /* 34: 日志级别*/
            /// </summary>
            Level,
            /// <summary>
            /* 37: */
            /// </summary>
            ResponseTime,
            /// <summary>
            /* 41: 创建时间*/
            /// </summary>
            Rec_CreateTime,
            /// <summary>
            /* 42: 创建人名称*/
            /// </summary>
            Rec_CreateBy,
        }
        #endregion
    }
}
