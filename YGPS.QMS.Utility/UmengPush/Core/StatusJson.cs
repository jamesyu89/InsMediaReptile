using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPhotos.Utility.UmengPush.Core
{
    public class StatusPostJson
    {
        public string appkey { get; set; }
        public string timestamp { get; set; }
        public string task_id { get; set; }
    }

    public class StatusReturnJson
    {
        public string ret { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string task_id { get; set; }
        /// <summary>
        /// 消息状态: 0-排队中, 1-发送中，2-发送完成，3-发送失败，4-消息被撤销，
        /// 5-消息过期, 6-筛选结果为空，7-定时任务尚未开始处理
        /// </summary>
        public int? status { get; set; }
        /// <summary>
        /// 消息总数
        /// </summary>
        public int? total_count { get; set; }
        /// <summary>
        /// 消息受理数
        /// </summary>
        public int? accept_count { get; set; }
        /// <summary>
        /// 消息实际发送数
        /// </summary>
        public int? sent_count { get; set; }
        /// <summary>
        /// 打开数
        /// </summary>
        public int? open_count { get; set; }
        /// <summary>
        /// 忽略数
        /// </summary>
        public int? dismiss_count { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string error_code { get; set; }

    }
}
