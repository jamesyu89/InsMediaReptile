using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace InstagramPhotos.Utility.Restful
{
    public class RestAPIExecutor : IRestAPIExecutor
    {
        public string BaseUrl { get; set; }

        public string DefaultDateParameterFormat { get; set; }

        public IAuthenticator DefaultAuthenticator { get; set; }

        private readonly RestClient client;

        public RestAPIExecutor(string CmsBaseURI, IAuthenticator Authenticator = null, string DateParameterFormat = null)
        {
            BaseUrl = CmsBaseURI;
            DefaultAuthenticator = Authenticator;

            client = new RestClient();

            if (DefaultAuthenticator != null)
                client.Authenticator = DefaultAuthenticator;

            if (DateParameterFormat != null)
                DefaultDateParameterFormat = DateParameterFormat;

            client.BaseUrl = new Uri(BaseUrl);
        }

        public T GenericExecute<T>(RestRequest request) where T : new()
        {
            request.DateFormat = string.IsNullOrEmpty(DefaultDateParameterFormat) ? "yyyy-MM-dd HH:mm:ss" : DefaultDateParameterFormat;

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                throw new Exception("Error retrieving response.  Check inner details for more info.", response.ErrorException);
            }

            return response.Data;
        }

        public RestRequestAsyncHandle AsyncGenericExecute<T>(RestRequest request, Action<IRestResponse<T>> action) where T : new()
        {
            request.DateFormat = string.IsNullOrEmpty(DefaultDateParameterFormat) ? "yyyy-MM-dd HH:mm:ss" : DefaultDateParameterFormat;
            return client.ExecuteAsync<T>(request, action);
        }

        public Task<T> GetTaskAsync<T>(RestRequest request) where T : new()
        {
            request.DateFormat = string.IsNullOrEmpty(DefaultDateParameterFormat) ? "yyyy-MM-dd HH:mm:ss" : DefaultDateParameterFormat;
            return client.GetTaskAsync<T>(request);
        }

        public IRestResponse Execute(RestRequest request)
        {
            request.DateFormat = string.IsNullOrEmpty(DefaultDateParameterFormat) ? "yyyy-MM-dd HH:mm:ss" : DefaultDateParameterFormat;

            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                throw new Exception("Error retrieving response.  Check inner details for more info.", response.ErrorException);
            }

            return response;
        }

        public byte[] DownloadData(RestRequest request)
        {
            return client.DownloadData(request);
        }

    }
}