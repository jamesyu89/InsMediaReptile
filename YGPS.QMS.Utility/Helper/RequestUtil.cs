using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using InstagramPhotos.Utility.Model;

namespace InstagramPhotos.Utility.Helper
{
    public class RequestUtil
    {
        #region 方法

        /// <summary>
        /// 获取客户端ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            var ipAddress = string.Empty;

            try
            {
                if (HttpContext.Current == null)
                {
                    return ipAddress;
                }

                var request = HttpContext.Current.Request;

                if (request.ServerVariables["HTTP_VIA"] != null)
                {
                    ipAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
                }
                else
                {
                    ipAddress = request.ServerVariables["REMOTE_ADDR"];
                }

                var userHostAddr = HttpContext.Current.Request.UserHostAddress;
                if (string.Compare("::1", ipAddress, true) == 0)
                {
                    ipAddress = userHostAddr;
                }
                if (string.Compare(ipAddress, userHostAddr, true) != 0)
                {
                    ipAddress = string.Format("{0}/{1}", ipAddress, userHostAddr);
                }
            }
            catch
            {
            }
            return ipAddress;
        }

        public static string GetServerIp()
        {
            var result = string.Empty;
            try
            {
                result = Dns.GetHostAddresses(string.Empty)
                   .First(x => x.IsIPv6LinkLocal == false && x.AddressFamily == AddressFamily.InterNetwork)
                   .ToString();
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// 获取本地计算机的主机名称
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            var result = string.Empty;
            try
            {
                result = Dns.GetHostName();
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// 获取当前网址的协议、Host及虚拟目录构成的字符串
        /// 形如：https://secure.xxx.com/realpay
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUrlWithAppPath(HttpContext context)
        {
            var url = string.Empty;
            try
            {
                if (context == null)
                {
                    return url;
                }
                var host = context.Request.Url.Host;
                var protocal = "https";
                if (context.Request.Url.AbsoluteUri.StartsWith("http:"))
                {
                    protocal = "http";
                }
                var port = context.Request.Url.Port.ToString();
                if (port == "80" || port == "443") //web默认端口
                {
                    port = string.Empty;
                }
                else
                {
                    port = ":" + port;
                }

                var appPath = UrlUtil.Decode(context.Request.ApplicationPath);
                if (string.IsNullOrWhiteSpace(appPath))
                {
                    url = string.Format("{0}://{1}{2}", protocal, host, port);
                }
                else if (appPath.Equals("/"))
                {
                    url = string.Format("{0}://{1}{2}", protocal, host, port);
                }
                else if (appPath.StartsWith("/"))
                {
                    url = string.Format("{0}://{1}{2}{3}", protocal, host, port, appPath);
                }
                else
                {
                    url = string.Format("{0}://{1}{2}/{3}", protocal, host, port, appPath);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return url;
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">远程访问地址 如：http://m.yaomaiche.com</param>
        /// <param name="sortedList">querystring参数字典 可以为空</param>
        /// <returns></returns>
        public static BizResult<string> HttpGet(string url, SortedList<string, string> sortedList, string charset = "UTF-8")
        {
            var resultInfo = new BizResult<string>(true);
            var respMsg = string.Empty;
            var respCode = 0;//响应码

            var encoder = Encoding.UTF8;

            HttpWebResponse response = null;
            try
            {
                if (string.IsNullOrWhiteSpace(charset) == false)
                {
                    encoder = Encoding.GetEncoding(charset);
                }

                var sb = new StringBuilder(256);
                if (sortedList != null)
                {
                    foreach (var kv in sortedList)
                    {
                        sb.AppendFormat("{0}={1}&", kv.Key, HttpUtility.UrlEncode(kv.Value, encoder));
                    }
                }

                var queryString = sb.ToString().Trim('&');
                if (queryString.Length > 0)
                {
                    if (url.LastIndexOf('?') == -1)
                    {
                        url = string.Format("{0}?{1}", url, queryString);
                    }
                    else
                    {
                        url = string.Format("{0}&{1}", url, queryString);
                    }
                }

                var request = (HttpWebRequest)WebRequest.Create(url);
                //request.Proxy = new WebProxy(ProxyString, true); //true means no proxy
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidate);

                response = (HttpWebResponse)request.GetResponse();
                var sr = new StreamReader(response.GetResponseStream(), encoder);

                respMsg = sr.ReadToEnd();

                respCode = Convert.ToInt32(response.StatusCode);

                sr.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                resultInfo.IsOK = false;
                if (response != null)
                {
                    respCode = Convert.ToInt32(response.StatusCode);
                }
                respMsg = string.Format("http响应码：{0}======异常信息:{1}", respCode, ex.ToString());
            }

            resultInfo.ReturnObject = respMsg;
            resultInfo.Code = respCode;

            return resultInfo;
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">远程访问地址 如：http://m.yaomaiche.com</param>
        /// <param name="sortedList">post表单参数字典 可以为空</param>
        /// <returns></returns>
        public static BizResult<string> HttpPost(string url, SortedList<string, string> sortedList, string charset = "UTF-8")
        {
            var resultInfo = new BizResult<string>(true);
            var respMsg = string.Empty;
            var respCode = 0;//响应码

            var encoder = Encoding.UTF8;

            HttpWebResponse response = null;
            try
            {
                if (string.IsNullOrWhiteSpace(charset) == false)
                {
                    encoder = Encoding.GetEncoding(charset);
                }

                var request = (HttpWebRequest)WebRequest.Create(url);
                //request.Proxy = new WebProxy(ProxyString, true); //true means no proxy
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidate);

                var sb = new StringBuilder(256);
                if (sortedList != null)
                {
                    foreach (var kv in sortedList)
                    {
                        sb.AppendFormat("{0}={1}&", kv.Key, HttpUtility.UrlEncode(kv.Value, encoder));
                    }
                }

                var postData = sb.ToString().Trim('&');

                if (postData.Length > 0)
                {
                    byte[] data = encoder.GetBytes(postData);

                    request.Method = "POST";

                    request.ContentType = "application/x-www-form-urlencoded";

                    request.ContentLength = data.Length;

                    Stream ws = request.GetRequestStream();

                    // 发送数据
                    ws.Write(data, 0, data.Length);
                    ws.Close();
                }

                response = (HttpWebResponse)request.GetResponse();
                var sr = new StreamReader(response.GetResponseStream(), encoder);

                respMsg = sr.ReadToEnd();

                respCode = Convert.ToInt32(response.StatusCode);

                sr.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                resultInfo.IsOK = false;
                if (response != null)
                {
                    respCode = Convert.ToInt32(response.StatusCode);
                }
                respMsg = string.Format("http响应码：{0}======异常信息:{1}", respCode, ex.ToString());
            }

            resultInfo.ReturnObject = respMsg;
            resultInfo.Code = respCode;

            return resultInfo;
        }

        public static BizResult<string> HttpPostXML(string xml, string url, bool isUseCert, int timeout,
            string charset = "UTF-8", string certPath = "", string sslPwd = "")
        {
            var resultInfo = new BizResult<string>(true);
            var respMsg = string.Empty;
            var respCode = 0;//响应码

            var encoder = Encoding.UTF8;

            HttpWebResponse response = null;
            try
            {
                if (string.IsNullOrWhiteSpace(charset) == false)
                {
                    encoder = Encoding.GetEncoding(charset);
                }

                //设置最大连接数
                ServicePointManager.DefaultConnectionLimit = 300;
                //设置https验证方式
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidate);
                }

                /***************************************************************
                * 下面设置HttpWebRequest的相关属性
                * ************************************************************/
                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                ////设置代理服务器
                //WebProxy proxy = new WebProxy();                          //定义一个网关对象
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              //网关服务器端口:端口
                //request.Proxy = proxy;

                //设置POST的数据类型和长度
                request.ContentType = "text/xml";
                byte[] data = encoder.GetBytes(xml);
                request.ContentLength = data.Length;

                //是否使用证书
                if (isUseCert)
                {
                    //var cert = new X509Certificate2(certPath, sslPwd);
                    //request.ClientCertificates.Add(cert);

                    try
                    {
                        var cert = new X509Certificate2(certPath, sslPwd);
                        request.ClientCertificates.Add(cert);
                    }
                    catch
                    {
                        X509Store store = new X509Store("My", StoreLocation.LocalMachine);
                        store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                        X509Certificate2 cert =
                            store.Certificates.Find(X509FindType.FindBySubjectName, sslPwd, false)[0];

                        request.ClientCertificates.Add(cert);
                    }
                }

                //往服务器写入数据
                var reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                var sr = new StreamReader(response.GetResponseStream(), encoder);
                respMsg = sr.ReadToEnd().Trim();
                sr.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                if (response != null)
                {
                    respCode = Convert.ToInt32(response.StatusCode);
                }
                respMsg = string.Format("http响应码：{0}======异常信息:{1}", respCode, ex.ToString());

                resultInfo.IsOK = false;
                resultInfo.Code = 0;
                resultInfo.Message = respMsg;
            }

            resultInfo.ReturnObject = respMsg;
            resultInfo.Code = respCode;

            return resultInfo;

        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 验证服务器证书
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidate(object sender,
            X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #endregion
    }
}