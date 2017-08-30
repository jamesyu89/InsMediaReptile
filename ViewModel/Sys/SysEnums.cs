using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPhotos.ViewModel.Sys
{
    /// <summary>
    /// 系统枚举
    /// </summary>
    public class SysEnums
    {
        /// <summary>
        /// 日志Level枚举
        /// </summary>
        public enum SysLogLevel
        {
            Fatal=1,
            Error,
            Warn,
            Info,
            Debug,
            Trace
        }
        /// <summary>
        /// 系统应用类型
        /// </summary>
        public enum SysAppType
        {
            Web=1,
            WebApi=2,
            GaeaService=3,
            GaeaAgent = 4,
            WinTask=5
        }
        
    }
}
