using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstagramPhotos.Utility.UmengPush.Core
{
    public interface IPush
    {
        ReturnJsonClass SendMessage(PostUMengJson paramsJsonObj);

        void AsynSendMessage(PostUMengJson paramsJsonObj, Action<ReturnJsonClass> callback);

    }
}
