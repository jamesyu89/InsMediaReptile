using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace YGOP.Task.Service.Umeng
{
    public class IosSender
    {
        #region 属性

        /// <summary>
        /// iOS相关
        /// </summary>
        public static string _iosKey = "53f6df9cfd98c54d3b001604";
        public static string _ios_app_master_secret = "3eswtobvqrhwouyiuvpvhszrsggno9z6";

        /// <summary>
        /// iPad
        /// </summary>
        public static string _ipadKey = "53f6dfbefd98c54d35001924";
        public static string _ipad_app_master_secret = "82dfdweiehti20xvmbwyx37flpbmq9kk";

        #endregion

        #region 发送消息
        /// <summary>
        /// iOS发送参数
        /// </summary>
        public class SendParms
        {
            /// <summary>
            /// 设备
            /// 当type=unicast时,必填, 表示指定的单个设备
            /// 当type=listcast时,必填,要求不超过500个,
            /// 多个device_token以英文逗号间隔 
            /// </summary>
            public string deviceTokens { get; set; }

            /// <summary>
            /// 类型 unicast-单播
            ///listcast-列播(要求不超过500个device_token)
            ///broadcast-广播
            ///groupcast-组播
            /// </summary>
            public string type { get; set; }

            /// <summary>
            /// 描述(ios)
            /// </summary>
            public string alert { get; set; }

            /// <summary>
            /// 描述
            /// </summary>
            public string description { get; set; }

            /// <summary>
            /// 签名
            /// </summary>
            public string sign { get; set; }

            /// <summary>
            /// 平台 iOS;Android;Ipad
            /// </summary>
            public string platform { get; set; }

            public string key { get; set; }

            public string secret { get; set; }

            /// <summary>
            /// 定时发送时间
            /// </summary>
            public DateTime start_time { get; set; }

            public DateTime expire_time { get; set; }

            public SendParms()
            {
                platform = "ios";

                if (platform.ToLower() == "ios")
                {
                    key = _iosKey;
                    secret = _ios_app_master_secret;
                }
                else if (platform.ToLower() == "ipad")
                {
                    key = _ipadKey;
                    secret = _ipad_app_master_secret;
                }

            }
            public int CityCode { get; set; }

            /// <summary>
            /// 标签
            /// </summary>
            public string Tag { get; set; }
        }


        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public RspJson Send(SendParms parms)
        {
            RspJson rspJson = new RspJson();
            try
            {
                string ts = UMengHelper.GetTimeStamp();
                SendJson json = new SendJson();
                json.appkey = parms.key;
                json.timestamp = ts;
                json.type = parms.type;
                json.device_tokens = parms.deviceTokens;
                payload payload = new payload();
                UMengHelper.policy policy = new UMengHelper.policy();
                policy.expire_time = parms.expire_time.ToString("yyyy-MM-dd HH:mm:ss");

                switch (parms.type)
                {
                    //单播
                    case "unicast":
                        policy.start_time = parms.start_time.ToString("yyyy-MM-dd HH:mm:ss");
                        break;

                    //广播
                    case "broadcast":
                        policy.start_time = parms.start_time.ToString("yyyy-MM-dd HH:mm:ss");
                        break;

                    //组播
                    case "groupcast":
                        policy.start_time = parms.start_time.ToString("yyyy-MM-dd HH:mm:ss");
                        json.filter = UMengHelper.GetFilterInfo(parms.CityCode,parms.Tag);
                        break;

                    default:
                        // display_type = "notification";
                        break;
                }

                if (parms.platform == "ios" || parms.platform == "ipad")
                {
                    payload.aps = new aps()
                    {
                        alert = parms.alert
                    };
                }

                json.policy = policy;
                payload.YGAction = parms.ygAction;
                json.payload = payload;
                json.production_mode = AppObjMrg.Config.UmengProduction; //是否生产环境-测试
                json.description = parms.description;
                JsonSerializerSettings jsetting = new JsonSerializerSettings();
                jsetting.DefaultValueHandling = DefaultValueHandling.Ignore;
                string str = JsonConvert.SerializeObject(json, Formatting.Indented, jsetting);

                string url = "http://msg.umeng.com/api/send";
                string mysign = UMengHelper.getMD5Hash("POST" + url + str + parms.secret);

                url = url + "?sign=" + mysign;
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";

                byte[] bs = Encoding.UTF8.GetBytes(str);
                request.ContentLength = bs.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bs, 0, bs.Length);
                    reqStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string resStr = sr.ReadToEnd();

                UMengHelper.ResponseUmeng res = JsonConvert.DeserializeObject<UMengHelper.ResponseUmeng>(resStr);
                if (res.ret == "SUCCESS")
                {
                    rspJson.RspCode = 1;
                    if (string.IsNullOrEmpty(res.data.msg_id))
                    {
                        rspJson.RspMsg = res.data.task_id;
                    }
                    else
                    {
                        rspJson.RspMsg = res.data.msg_id;
                    }
                    LogManager.WriteAppWork("SendUmengSuccess", string.Format("友盟消息发送成功：body：{0}", str));
                }
                else
                {
                    rspJson.RspCode = 0;
                    rspJson.RspMsg = res.data.error_code;
                    LogManager.WriteAppError(ErrorLevel.High, string.Format("友盟发送失败，原因{0}", res.data.error_code));
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Stream myResponseStream = ((HttpWebResponse)e.Response).GetResponseStream();
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                    string retString = myStreamReader.ReadToEnd();
                    LogManager.WriteAppError(ErrorLevel.High, string.Format("友盟发送异常，原因{0}", retString));
                    rspJson.RspCode = 2;
                    rspJson.RspMsg = retString;
                }
            }
            return rspJson;
        }

        /// <summary>
        /// 友盟传值参数
        /// </summary>
        public class SendJson
        {
            /// <summary>
            /// 应用key
            /// </summary>
            public string appkey { get; set; }

            /// <summary>
            /// 时间戳
            /// </summary>
            public string timestamp { get; set; }

            /// <summary>
            /// 消息发送类型
            //unicast-单播
            //listcast-列播(要求不超过500个device_token)
            //filecast-文件播 (多个device_token可通过文件形式批量发送）
            //broadcast-广播
            //groupcast-组播
            /// </summary>
            public string type { get; set; }

            /// <summary>
            //当type=unicast时,必填, 表示指定的单个设备
            //当type=listcast时,必填,要求不超过500个,
            //多个device_token以英文逗号间隔
            /// </summary>
            public string device_tokens { get; set; }

            public payload payload { get; set; }

            public UMengHelper.policy policy { get; set; }


            //筛选条件
            public UMengHelper.filter filter { get; set; }

            /// <summary>
            /// 是否生成环境
            /// </summary>
            public string production_mode { get; set; }

            public string description { get; set; }
        }

        /// <summary>
        /// apns
        /// </summary>
        public class aps
        {
            public string alert { get; set; }
        }

        #endregion


        #region 公用



        public class payload
        {
            /// <summary>
            /// iOS 专用
            /// </summary>
            public aps aps { get; set; }

            /// <summary>
            /// Android专用：消息类型，值可以为: notification-通知，message-消息
            /// </summary>

            public UMengHelper.YGAction YGAction { get; set; }
        }
        #endregion
    }
}