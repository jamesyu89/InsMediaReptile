using InstagramPhotos.ViewModel;
using Service.BLL;
using Service.Interface.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPhotos.Framework.Common
{
    /// <summary>
    /// 扩展日志的内容写入
    /// </summary>
    public static class StringLog
    {
        static readonly IMediaService mediaService = ServiceFactory.GetInstance<IMediaService>();

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="str">日志内容</param>
        /// <param name="addToQueue">是否添加到自定义消息队列</param>
        /// <returns></returns>
        public static string Log(this string str, bool addToQueue = false)
        {
            mediaService.AddDownloadlog(new DownloadLogEntity
            {
                LogId = Guid.NewGuid(),
                SortValue = DateTime.Now.ToString("yyyyMMddhhmmssfff"),
                Disabled = 0,
                Level = 0,
                Message = str,
                Rec_CreateBy = Guid.Parse("3102A7AC-35DF-4C9C-8A11-CE9501EBE300"),
                Rec_CreateTime = DateTime.Now
            });
            if (addToQueue)
            {
                DefineMessageQueue.AddToQueue(str);
            }
            return str;
        }
    }
}
