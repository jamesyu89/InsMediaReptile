using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// URL/HTML������
    /// </summary>
    public class UrlUtil
    {
        #region fields

        /// <summary>
        /// ���������ֶ��б� ��¼��־Ϊ$$$$$$����ֹ������Ϣй��
        /// </summary>
        private static readonly IList<string> sensitiveFields = new List<string>
        {
            "cardnumber",
            "last4number",
            "cvv2",
            "validity",
            "verifycode",
            "upopsmscode",
            "valuesmscode",
            "mobile",
        };

        #endregion

        #region url encode/decode

        /// <summary>
        /// ��ȡURL�����ַ���
        /// </summary>
        /// <param name="text">ԭ��</param>
        /// <returns>����ַ���</returns>
        public static string Encode(string text)
        {
            if (string.IsNullOrEmpty(text) == false)
            {
                return HttpUtility.UrlEncode(text, Encoding.UTF8);
            }

            return string.Empty;
        }

        /// <summary>
        /// ��ȡURL�����ַ���
        /// </summary>
        /// <param name="text">ԭ��</param>
        /// <returns>����ַ���</returns>
        public static string Decode(string text)
        {
            if (string.IsNullOrEmpty(text) == false)
            {
                return HttpUtility.UrlDecode(text, Encoding.UTF8);
            }

            return string.Empty;
        }

        /// <summary>
        /// ��ȡBase64�����ַ���
        /// </summary>
        /// <param name="text">ԭ��</param>
        /// <returns>����ַ���</returns>
        public static string EncodeBase64(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        /// ��ȡBase64�����ַ���
        /// </summary>
        /// <param name="text">ԭ��</param>
        /// <returns>����ַ���</returns>
        public static string DecodeBase64(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }

        #endregion

        #region query func

        /// <summary>
        /// ��ȡParams��ֵ
        /// </summary>
        /// <param name="request"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string GetParamsValue(HttpRequest request, string paramName)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Params[paramName]))
                {
                    return string.Empty;
                }

                return HttpUtility.HtmlEncode(request.Params[paramName].Trim());
            }
            catch
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// ��ȡParams��ֵ
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string GetParamsValue(string queryString, string paramName)
        {
            try
            {
                if (string.IsNullOrEmpty(queryString))
                {
                    return string.Empty;
                }

                if (string.IsNullOrEmpty(paramName))
                {
                    return queryString;
                }

                var collection = GetQueryString(queryString);

                return collection[paramName] ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// ��ȡ�����ַ��� ������ǰ�ͽ����
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static string GetCollectionString(NameValueCollection collection)
        {
            var result = string.Empty;
            try
            {
                if (collection == null)
                {
                    return result;
                }
                var sb = new StringBuilder(512);
                sb.AppendFormat("����ǰ������");
                foreach (var item in collection.AllKeys)
                {
                    sb.AppendFormat("{0}:{1} ", item, collection[item]);
                }
                sb.AppendLine();
                sb.Append("==========����������");
                foreach (var item in collection.AllKeys)
                {
                    sb.AppendFormat("{0}:{1} ", item, UrlUtil.Decode(collection[item]));
                }
                result = sb.ToString();
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// ��ȡ�����ַ��� ���ݲ����ж��ǻ�ȡ����ǰ���ǽ����
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="isDecode">�Ƿ��ǽ������</param>
        /// <returns></returns>
        public static string GetCollectionString(NameValueCollection collection, bool isDecode = true)
        {
            var result = string.Empty;
            try
            {
                if (collection == null)
                {
                    return result;
                }
                var sb = new StringBuilder(512);
                if (isDecode == false)
                {
                    foreach (var item in collection.AllKeys)
                    {
                        sb.AppendFormat("{0}:{1} ", item, collection[item]);
                    }
                }
                else //�������
                {
                    foreach (var item in collection.AllKeys)
                    {
                        sb.AppendFormat("{0}:{1} ", item, UrlUtil.Decode(collection[item]));
                    }
                }
                result = sb.ToString();
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// ��ȡweb���������Ϣ ������ǰ�ͽ����
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRequestString(HttpRequest request)
        {
            if (request == null)
            {
                return string.Empty;
            }
            var sb = new StringBuilder(1024);
            try
            {
                sb.AppendFormat(@"��ClientIP:{0}������URL:{1}��,��UrlReferrer:{2}��,UserAgent:{3}��",
                    RequestUtil.GetClientIP(), request.Url, request.UrlReferrer, request.UserAgent);

                var collection = request.QueryString;
                if (collection != null && collection.Count > 0)
                {
                    sb.AppendFormat("QueryString:{0}", GetCollectionString(request.QueryString));
                }
                sb.AppendLine();
                collection = request.Form;
                if (collection != null && collection.Count > 0)
                {
                    sb.AppendFormat("Request.Form:{0}", GetCollectionString(request.Form));
                }
            }
            catch
            {
            }
            return sb.ToString();
        }

        /// <summary>
        /// �����ϼ�ֵƴ�ӳ�querystring����ʽ
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static string GetQueryString(NameValueCollection collection)
        {
            var result = string.Empty;
            if (collection == null || collection.Count == 0)
            {
                return result;
            }

            var sb = new StringBuilder(1024);
            foreach (var item in collection.AllKeys)
            {
                sb.AppendFormat("&{0}={1}", item, collection[item]);
            }

            result = sb.ToString().Substring(1);

            return result;
        }

        /// <summary>
        /// ��ȡweb���������Ϣ ������ǰ�ͽ����������Ϣ�翨�ŵȲ�����
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetStringNoSensitiveInfo(HttpRequest request)
        {
            if (request == null)
            {
                return string.Empty;
            }
            var sb = new StringBuilder(1024);
            try
            {
                sb.AppendFormat(@"��ClientIP:{0}������URL:{1}��,��UrlReferrer:{2}��,UserAgent:{3}��",
                    RequestUtil.GetClientIP(), request.Url, request.UrlReferrer, request.UserAgent);

                var collection = request.QueryString;
                if (collection != null && collection.Count > 0)
                {
                    sb.AppendFormat("QueryString:{0}", GetCollectStringNoSensitiveInfo(request.QueryString));
                }
                sb.AppendLine();
                collection = request.Form;
                if (collection != null && collection.Count > 0)
                {
                    sb.AppendFormat("Request.Form:{0}", GetCollectStringNoSensitiveInfo(request.Form));
                }
            }
            catch
            {
            }
            return sb.ToString();
        }

        /// <summary>
        /// ��ȡ�����ַ��� ������ǰ�ͽ���� ������Ϣ�翨�ŵȲ�����
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private static string GetCollectStringNoSensitiveInfo(NameValueCollection collection)
        {
            var result = string.Empty;
            try
            {
                if (collection == null)
                {
                    return result;
                }
                var sb = new StringBuilder(512);
                sb.AppendFormat("����ǰ������");
                foreach (var item in collection.AllKeys)
                {
                    if (sensitiveFields.Where(x => string.Compare(x, item, true) == 0).Any() == true)
                    {
                        sb.AppendFormat("{0}:{1} ", item, "$$$$$$");
                    }
                    else
                    {
                        sb.AppendFormat("{0}:{1} ", item, collection[item]);
                    }
                }
                sb.AppendLine();
                sb.Append("==========����������");
                foreach (var item in collection.AllKeys)
                {
                    if (sensitiveFields.Where(x => string.Compare(x, item, true) == 0).Any() == true)
                    {
                        sb.AppendFormat("{0}:{1} ", item, "$$$$$$");
                    }
                    else
                    {
                        sb.AppendFormat("{0}:{1} ", item, UrlUtil.Decode(collection[item]));
                    }
                }
                result = sb.ToString();
            }
            catch
            {
            }
            return result;
        }

        #endregion

        #region NameValueCollection

        ///<summary>
        ///����ѯ�ַ�������ת��Ϊ��ֵ����.
        ///</summary>
        ///<param name="queryString">��ѯ�ַ�����ֵ</param>
        ///<returns>���</returns>
        public static NameValueCollection GetQueryString(string queryString)
        {
            var result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(queryString))
            {
                return result;
            }

            string key = string.Empty;
            string value = string.Empty;
            string[] items = queryString.Split('&');
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item) == true)
                {
                    continue;
                }

                string[] fields = item.Split('=');
                if (fields != null && fields.Length == 2)
                {
                    key = fields[0].Trim();
                    value = fields[1].Trim();

                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }

                    result[key] = value;
                }
            }

            return result;
        }

        ///<summary>
        ///����ѯ�ַ�������ת��Ϊ����List����ֵ����.
        ///</summary>
        ///<param name="queryString">��ѯ�ַ�����ֵ</param>
        ///<returns>���</returns>
        public static List<NameValueCollection> GetMultipleRecords(string records)
        {
            var result = new List<NameValueCollection>();
            if (string.IsNullOrEmpty(records))
            {
                return result;
            }

            var items = records.Split('|');
            if (items == null || items.Length == 0)
            {
                return result;
            }

            foreach (string item in items)
            {
                var current = GetQueryString(item);
                if (current.Count == 0)
                {
                    continue;
                }

                result.Add(current);
            }

            return result;
        }

        ///<summary>
        ///��������¼�ַ�������ת��Ϊ��ֵ����.
        ///</summary>
        ///<param name="queryString">������¼�ַ���</param>
        ///<param name="splitChar">�ָ����</param>
        ///<returns>���</returns>
        public static NameValueCollection GetMultipleRecords(string queryString, char splitChar)
        {
            var result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(queryString))
            {
                return result;
            }

            string key = string.Empty;
            string value = string.Empty;
            string[] items = queryString.Split(splitChar);
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                string[] fields = item.Split('=');
                if (fields != null && fields.Length == 2)
                {
                    key = fields[0].Trim();
                    value = fields[1].Trim();

                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }

                    result[key] = value;
                }
            }

            return result;
        }

        /// <summary>
        /// �ϲ�������������
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static NameValueCollection Merge(NameValueCollection left, NameValueCollection right)
        {
            var result = new NameValueCollection();
            if (left == null)
            {
                left = new NameValueCollection();
            }
            if (right == null)
            {
                right = new NameValueCollection();
            }
            foreach (var item in left.AllKeys)
            {
                result[item] = left[item];
            }
            foreach (var item in right.AllKeys)
            {
                result[item] = right[item];
            }

            return result;
        }

        ///<summary>
        ///����ѯ�ַ�������ת��Ϊ��ֵ����. ֧����RSA�ص������=�����������ַ� �����⴦��
        ///</summary>
        ///<param name="queryString">��ѯ�ַ�����ֵ</param>
        ///<returns>���</returns>
        public static NameValueCollection GetQueryStringForRSA(string queryString)
        {
            var result = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrEmpty(queryString))
            {
                return result;
            }

            string key = string.Empty;
            string value = string.Empty;
            string[] items = queryString.Split('&');
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item) == true)
                {
                    continue;
                }

                string[] fields = item.Split('=');
                if (fields != null && fields.Length == 2)
                {
                    key = fields[0].Trim();
                    value = fields[1].Trim();

                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }

                    result[key] = value;
                }
                else if (fields.Length > 2) //ȡ��һ��=�ź���Ĳ���
                {
                    key = fields[0].Trim();
                    value = item.Substring(key.Length + 1);

                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }

                    result[key] = value;
                }
            }

            return result;
        }

        #endregion

    }
}