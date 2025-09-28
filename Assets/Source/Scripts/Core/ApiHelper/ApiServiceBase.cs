using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine.Networking;
using Observable = R3.Observable;

namespace Source.Scripts.Core.ApiHelper
{
    internal abstract class ApiServiceBase<TConfig> : IDisposable where TConfig : ApiConfigBase
    {
        public ReadOnlyReactiveProperty<bool> IsAvailable => _isAvailable;
        private readonly ReactiveProperty<bool> _isAvailable = new();

        private readonly IApiAvailabilityChecker _apiAvailabilityChecker;
        private readonly IApiClient _apiClient;
        private readonly TConfig _config;

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly IDisposable _disposable;

        internal ApiServiceBase(IApiAvailabilityChecker apiAvailabilityChecker, IApiClient apiClient, TConfig config)
        {
            _apiAvailabilityChecker = apiAvailabilityChecker;
            _apiClient = apiClient;
            _config = config;

            _disposable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(_config.UpdateAvailabilityInterval))
                .AsObservable()
                .Subscribe(this, static (_, self) => self.UpdateAvailable(self._cancellationTokenSource.Token)
                    .Forget());
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

        public async UniTask UpdateAvailable(CancellationToken token)
        {
            _isAvailable.Value = await _apiAvailabilityChecker.IsAvailable(
                _config.AvailabilityCheckUrl, _config.AvailabilityCode, token);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
            _isAvailable.Dispose();
            _disposable.Dispose();
        }
    }
}