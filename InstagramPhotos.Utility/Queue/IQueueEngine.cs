using Jns.BasicService.MessageCenter;

namespace InstagramPhotos.Utility.Queue
{
    public interface IQueueEngine
    {

        /// <summary>
        ///     异步获取队列消息
        ///      ConsumerMessage.ConsumerMessageHandler void xxx(string msg)
        /// </summary>
        /// <param name="queue_name"></param>
        /// <param name="AsyncCallBack"></param>
        /// <returns></returns>
        bool ReceiveMessageAsync(string queue_name, ConsumerMessage.ConsumerMessageHandler AsyncCallBack);
    }
}