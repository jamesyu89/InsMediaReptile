using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPhotos.Framework.Common
{
    /// <summary>
    /// 自定义消息队列
    /// </summary>
    public class DefineMessageQueue
    {
        public static DefineMessageQueue Instance()
        {
            return new DefineMessageQueue();
        }

        /// <summary>
        /// 自定义消息队列
        /// </summary>
        public Queue<string> ItemQueue { get; set; }

        /// <summary>
        /// 添加消息到自定义队列
        /// </summary>
        /// <param name="message"></param>
        public void AddToQueue(string message)
        {
            ItemQueue.Enqueue(message);
        }

        /// <summary>
        /// 取出队列里的消息
        /// </summary>
        /// <returns></returns>
        public string OutFromQueue()
        {
            if (ItemQueue.Count > 0)
            {
                return ItemQueue.Dequeue();
            }
            return string.Empty;
        }
    }
}
