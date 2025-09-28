using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine.Networking;
using Observable = R3.Observable;

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

            Observable.Interval(TimeSpan.FromSeconds(_config.UpdateAvailabilityInterval))
                .AsObservable()
                .Subscribe(this, static (_, self) => self.UpdateAvailable(CancellationToken.None).Forget());
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

        public abstract UniTask UpdateAvailable(CancellationToken token);
    }
}