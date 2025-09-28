using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Api.Interfaces;
using UnityEngine.Networking;
using Observable = R3.Observable;

namespace Source.Scripts.Core.Api.Base
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

        internal async UniTask<ResponseResult<TResponse>> GetResponse<TRequest, TResponse>(
            TRequest request,
            CancellationToken token)
            where TRequest : class
            where TResponse : class, IValidatable
        {
            if (_config.IsValid() is false)
                return new ResponseResult<TResponse>(false);

            var url = _config.GetApiUrl();
            var response = await _apiClient.PostAsync<TRequest, TResponse>(request, url, token);
            var success = response != null && response.IsValid();
            return new ResponseResult<TResponse>(response, success);
        }

        internal async UniTask<ResponseResult<TResponse>> GetResponse<TSource, TRequest, TResponse>(
            TSource source,
            TRequest request,
            CancellationToken token,
            Action<TSource, UnityWebRequest> additionalHeaders)
            where TRequest : class
            where TResponse : class, IValidatable
        {
            if (_config.IsValid() is false)
                return new ResponseResult<TResponse>(false);

            var url = _config.GetApiUrl();
            var response = await _apiClient.PostAsync<TSource, TRequest, TResponse>(
                source,
                request,
                url,
                token,
                additionalHeaders);
            var success = response != null && response.IsValid();
            return new ResponseResult<TResponse>(response, success);
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