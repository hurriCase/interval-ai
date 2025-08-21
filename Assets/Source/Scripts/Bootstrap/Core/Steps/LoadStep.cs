using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Others;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Bootstrap.Core.Steps
{
    [CreateAssetMenu(
        fileName = nameof(LoadStep),
        menuName = InitializationStepsPath + nameof(LoadStep)
    )]
    internal sealed class LoadStep : StepBase
    {
        [Inject] private IReadOnlyList<ILoadable> _entitiesToLoad;
        [Inject] private IObjectResolver _objectResolver;

        protected override async UniTask ExecuteInternal(CancellationToken token)
        {
            var configsList = _entitiesToLoad.ToList();
            var tasks = new UniTask[configsList.Count];

            for (var i = 0; i < configsList.Count; i++)
            {
                _objectResolver.Inject(configsList[i]);
                tasks[i] = configsList[i].InitAsync(token);
            }

            await UniTask.WhenAll(tasks);
        }
    }
}