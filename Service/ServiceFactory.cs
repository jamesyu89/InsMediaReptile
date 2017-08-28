using InstagramPhotos.Utility.KVStore;
using InstagramPhotos.Utility.Queue;
using InstagramPhotos.Utility.Queue.JnsMessageCenter;
using LightInject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.BLL
{
    public class ServiceFactory
    {
        private static readonly ServiceContainer container = new ServiceContainer();

        static ServiceFactory()
        {
            //utility
            container.Register<IKvStoreRedisEngine, KVStoreEngine>();
            container.Register<IQueueEngine, JMCEngine>();

        }

        public static T GetInstance<T>()
        {
            return container.GetInstance<T>();
        }
    }
}
