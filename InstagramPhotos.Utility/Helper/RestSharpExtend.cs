using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace InstagramPhotos.Utility.Helper
{
    public static class RestSharpExtend
    {
        public static EasyApiResponse<T> GetResponse<T>(this IRestResponse restResponse)
        {
            var response = new EasyApiResponse<T>
            {
                StatusCode = restResponse.StatusCode,
            };

            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                response.Data = JsonConvert.DeserializeObject<T>(restResponse.Content);
            }

            return response;
        }
    }

    public class EasyApiResponse<T>
    {   
        public HttpStatusCode StatusCode { get; set; }

        public T Data { get; set; }
    }
}