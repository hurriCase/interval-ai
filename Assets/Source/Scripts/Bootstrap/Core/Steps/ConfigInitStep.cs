using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using FirebaseServices.Runtime.Config;
using Source.Scripts.Core.Api.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Bootstrap.Core.Steps
{
    [CreateAssetMenu(
        fileName = nameof(ConfigInitStep),
        menuName = InitializationStepsPath + nameof(ConfigInitStep)
    )]
    internal sealed class ConfigInitStep : StepBase
    {
        private IRemoteConfigService _remoteConfigService;
        private IEnumerable<ApiConfigBase> _apiConfigs;

        [Inject]
        internal void Inject(IRemoteConfigService remoteConfigService, IEnumerable<ApiConfigBase> apiConfigs)
        {
            _remoteConfigService = remoteConfigService;
            _apiConfigs = apiConfigs;
        }

        protected override async UniTask ExecuteInternal(CancellationToken token)
        {
            await _remoteConfigService.InitAsync(token);

            foreach (var apiConfigBase in _apiConfigs)
                apiConfigBase.Init(_remoteConfigService);
        }
    }
}