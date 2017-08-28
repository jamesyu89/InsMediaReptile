using System;
using System.Collections.Generic;
using System.Linq;
using Jns.BasicService.MessageCenter;
using InstagramPhotos.Utility.Configuration;
using InstagramPhotos.Utility.Log;

namespace InstagramPhotos.Utility.Queue.JnsMessageCenter
{
    public class JMCEngine : IQueueEngine
    {
        #region member

        private readonly string uri = AppSettings.GetValue("JnsQueue_ApiUrl", "amqp://172.17.7.207:5672/order");
        private readonly string userName = AppSettings.GetValue("JnsQueue_Name", "order");
        private readonly string passWord = AppSettings.GetValue("JnsQueue_Pwd", "order_123");
        private readonly string exChange = AppSettings.GetValue("JnsQueue_Exchange", "qms_exchange");
        private readonly string routingKey = AppSettings.GetValue("JnsQueue_RouteKey", "qms_routingkey");
        private readonly string queueName = AppSettings.GetValue("JnsQueue_Queue", "qms_queue");
        private ProducerMessage producerMessage;
        #endregion

        public JMCEngine()
        {
            producerMessage = new ProducerMessage(uri, userName, passWord, exChange);
        }
        #region Method

        /// <summary>
        ///     异步获取队列消息
        ///      ConsumerMessage.ConsumerMessageHandler void xxx(string msg)
        /// </summary>
        /// <param name="queue_name"></param>
        /// <param name="AsyncCallBack"></param>
        /// <returns></returns>
        public Boolean ReceiveMessageAsync(string queue_name, ConsumerMessage.ConsumerMessageHandler AsyncCallBack)
        {
            try
            {
                var client = new ConsumerMessage(uri, userName, passWord, queue_name);
                client.ConsumerMessageEvent += AsyncCallBack;
                return true;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
                throw;
            }
        }

        public bool SendMessage(string message, string exchange=null, string routingkey = null)
        {
            if(string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));
            return producerMessage.SendMessage(routingkey ?? routingKey??"", message);
        }


        public void SendMessageNonConfirm(string message, string exchange=null, string routingkey = null)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));
            producerMessage.SendMessageNoConfirm(routingkey ?? routingKey ?? "", message);
        }


        //public bool SendMessageList(List<string> messagelist, string exchange = null, string routingkey = null)
        //{
        //    if (!messagelist.Any())
        //        throw new ArgumentException("messagelist不能为空");
        //    return producerMessage.SendMessage( routingkey ?? routingKey ?? "", messagelist);
        //}


        //public void SendMessageListNonConfirm(List<string> messagelist, string exchange = null, string routingkey = null)
        //{
        //    if (!messagelist.Any())
        //        throw new ArgumentException("messagelist不能为空");
        //    producerMessage.SendMessageNoConfirm(exchange ?? exChange, routingkey ?? routingKey ?? "", messagelist);
        //}
        #endregion
    }
}