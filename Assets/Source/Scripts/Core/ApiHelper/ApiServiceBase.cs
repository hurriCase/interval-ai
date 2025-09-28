using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Source.Scripts.Core.ApiHelper
{
    internal abstract class ApiServiceBase<TConfig> where TConfig : ApiConfigBase
    {
        private readonly IApiHelper _apiHelper;
        private readonly TConfig _config;

        internal ApiServiceBase(IApiHelper apiHelper, TConfig config)
        {
            _apiHelper = apiHelper;
            _config = config;
        }

        internal async UniTask<TResponse> GetResponse<TRequest, TResponse>(TRequest request, CancellationToken token)
            where TRequest : class
            where TResponse : class
        {
            if (_config.IsValidUrl() is false)
                return null;

            var url = _config.GetApiUrl();
            return await _apiHelper.PostAsync<TRequest, TResponse>(request, url, token);
        }

        internal async UniTask<TResponse> GetResponse<TSource, TRequest, TResponse>(
            TSource source,
            TRequest request,
            CancellationToken token,
            Action<TSource, UnityWebRequest> additionalHeaders)
            where TRequest : class
            where TResponse : class
        {
            if (_config.IsValidUrl() is false)
                return null;

            var url = _config.GetApiUrl();
            return await _apiHelper.PostAsync<TSource, TRequest, TResponse>(
                source,
                request,
                url,
                token,
                additionalHeaders);
        }
    }
}