using InstagramPhotos.Utility.KVStore;
//using InstagramPhotos.Utility.Queue;
//using InstagramPhotos.Utility.Queue.JnsMessageCenter;
using LightInject;
using Service.BLL.Media;
using Service.Interface.Media;

namespace Service.BLL
{
    public class ServiceFactory
    {
        private static readonly ServiceContainer container = new ServiceContainer();

        static ServiceFactory()
        {
            //utility
            container.Register<IKvStoreRedisEngine, KVStoreEngine>();
            //container.Register<IQueueEngine, JMCEngine>();

            //media
            container.Register<IMediaService, MediaService>();


        }

        public static T GetInstance<T>()
        {
            return container.GetInstance<T>();
        }
    }
}
