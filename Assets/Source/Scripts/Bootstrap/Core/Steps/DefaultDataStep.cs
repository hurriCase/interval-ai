using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Bootstrap.Core.Steps
{
    [CreateAssetMenu(
        fileName = nameof(DefaultDataStep),
        menuName = InitializationStepsPath + nameof(DefaultDataStep)
    )]
    internal sealed class DefaultDataStep : StepBase
    {
        [Inject] private IEnumerable<IDefaultConfig> _defaultConfigs;
        [Inject] private IObjectResolver _objectResolver;

        protected override async UniTask ExecuteInternal(CancellationToken token)
        {
            foreach (var defaultConfig in _defaultConfigs)
            {
                _objectResolver.Inject(defaultConfig);

                await defaultConfig.InitAsync(token);
            }
        }
    }
}