using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPhotos.Utility.Utility
{
    public class RequestTools
    {
        public static string UserAgent { get; set; }

        public static string ContentType { get; set; }

        /// <summary>
        ///     get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            try
            {
                string html = string.Empty;
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.111 Safari/537.36";
                request.Timeout = 3000;
                request.Proxy = null; // SetWebProxy();
                request.Method = "GET";
                var response = request.GetResponse() as HttpWebResponse;
                using (Stream stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8")))
                    {
                        html = reader.ReadToEnd();
                    }
                }
                response.Close();
                if (request != null)
                {
                    request.Abort();
                }
                return html;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static string Get(string url, string userAgent = "")
        {
            try
            {
                string html = string.Empty;
                var request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent = userAgent;
                request.Method = "GET";
                var response = request.GetResponse() as HttpWebResponse;
                using (Stream stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream, Encoding.GetEncoding("UTF-8")))
                    {
                        html = reader.ReadToEnd();
                    }
                }
                response.Close();
                if (request != null)
                {
                    request.Abort();
                }
                return html;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        ///     Post请求
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string Post(string url, string postData = "", string encoding = "UTF-8")
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = ContentType ?? "application/x-www-form-urlencoded; charset=utf-8";
                request.UserAgent = UserAgent ??
                                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.111 Safari/537.36";
                if (!string.IsNullOrEmpty(postData))
                {
                    byte[] data = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = data.Length;
                    using (Stream newStream = request.GetRequestStream())
                    {
                        newStream.Write(data, 0, data.Length);
                    }
                }
                var response = request.GetResponse() as HttpWebResponse;
                using (var strReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding)))
                {
                    return strReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public static ResponseResult GetRequest(string url, CookieCollection cookies = null)
        {
            var result = new ResponseResult();
            result.Url = url;
            var handler = new HttpClientHandler {AutomaticDecompression = DecompressionMethods.GZip};
            if (cookies != null)
            {
                handler.CookieContainer.Add(cookies);
            }
            using (var http = new HttpClient(handler))
            {
                Task<HttpResponseMessage> response = http.GetAsync(url);
                response.Wait();
                //string[] strCookies = (string[])response.Result.Headers.GetValues("Set-Cookie");
                result.Cookies = handler.CookieContainer.GetCookies(new Uri(url));
                result.Html = response.Result.Content.ReadAsStringAsync().Result;
            }
            return result;
        }


        public static ResponseResult PostRequest(string url, Dictionary<string, string> param,
            Dictionary<string, string> headers = null, CookieCollection cookes = null)
        {
            var result = new ResponseResult();
            result.Url = url;
            var handler = new HttpClientHandler {AutomaticDecompression = DecompressionMethods.GZip};
            if (cookes != null)
            {
                handler.CookieContainer.Add(cookes);
            }
            var requestBody = new FormUrlEncodedContent(param);
            using (var http = new HttpClient(handler))
            {
                if (headers != null)
                {
                    foreach (var h in headers)
                    {
                        http.DefaultRequestHeaders.Add(h.Key, h.Value);
                    }
                }
                Task<HttpResponseMessage> response = http.PostAsync(url, requestBody);
                response.Wait();
                result.Cookies = handler.CookieContainer.GetCookies(new Uri(url));
                result.Html = response.Result.Content.ReadAsStringAsync().Result;
            }
            return result;
        }

        public static string Url(string host, string path)
        {
            return string.Format("{0}/{1}", host, path);
        }
    }


    public class ResponseResult
    {
        public CookieCollection Cookies { get; set; }

        public string Html { get; set; }

        public string Url { get; set; }
    }
}