using System;
using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Source.Scripts.Core.ApiHelper
{
    internal abstract class ApiServiceBase<TConfig> where TConfig : ApiConfigBase
    {
        private readonly IApiClient _apiClient;
        private readonly TConfig _config;

        internal ApiServiceBase(IApiClient apiClient, TConfig config)
        {
            _apiClient = apiClient;
            _config = config;
        }

        internal async UniTask<TResponse> GetResponse<TRequest, TResponse>(TRequest request, CancellationToken token)
            where TRequest : class
            where TResponse : class
        {
            if (_config.IsValidUrl() is false)
                return null;

            var url = _config.GetApiUrl();
            return await _apiClient.PostAsync<TRequest, TResponse>(request, url, token);
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
            return await _apiClient.PostAsync<TSource, TRequest, TResponse>(
                source,
                request,
                url,
                token,
                additionalHeaders);
        }
    }
}