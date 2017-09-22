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
    public static class DefineMessageQueue
    {
        /// <summary>
        /// 自定义消息队列
        /// </summary>
        static public Queue<string> ItemQueue = new Queue<string>();

        /// <summary>
        /// 添加消息到自定义队列
        /// </summary>
        /// <param name="message"></param>
        static public void AddToQueue(string message)
        {
            ItemQueue.Enqueue(message);
        }

        /// <summary>
        /// 取出队列里的消息
        /// </summary>
        /// <returns></returns>
        static public string OutFromQueue()
        {
            if (ItemQueue.Count > 0)
            {
                return ItemQueue.Dequeue();
            }
            return string.Empty;
        }
    }
}
