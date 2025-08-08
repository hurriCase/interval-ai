using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base.DefaultConfig;
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
        [Inject] private IEnumerable<IDefaultDatabase> _defaultConfigs;
        [Inject] private IObjectResolver _objectResolver;

        protected override async UniTask ExecuteInternal(CancellationToken token)
        {
            var tasks = new List<UniTask>();

            foreach (var defaultConfig in _defaultConfigs)
            {
                _objectResolver.Inject(defaultConfig);

                tasks.Add(defaultConfig.InitAsync(token));
            }

            await UniTask.WhenAll(tasks);
        }
    }
}