using System;
using SSOUtil;

namespace InstagramPhotos.Utility.YIGUO
{
    public class WCFDBConn
    {
        public string GetDbConnectString(string Keys)
        {
            string result;
            try
            {
                result = WCFChannel.GetWCFChannle().Get_DBConnection(Keys);
            }
            catch (Exception)
            {
                WCFChannel.ReConnectChannle();
                result = WCFChannel.GetWCFChannle().Get_DBConnection(Keys);
            }
            return result;
        }
    }
}