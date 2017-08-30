using System;
using System.Threading.Tasks;
using RestSharp;

namespace InstagramPhotos.Utility.Restful
{
    public interface IRestAPIExecutor
    {
        T GenericExecute<T>(RestRequest request) where T : new();

        RestRequestAsyncHandle AsyncGenericExecute<T>(RestRequest request, Action<IRestResponse<T>> action)
            where T : new();

        Task<T> GetTaskAsync<T>(RestRequest request) where T : new();

        IRestResponse Execute(RestRequest request);

        byte[] DownloadData(RestRequest request);
    }
}
