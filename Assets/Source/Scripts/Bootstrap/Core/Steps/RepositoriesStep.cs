using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Data.Repositories;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Bootstrap.Core.Steps
{
    [CreateAssetMenu(
        fileName = nameof(RepositoriesStep),
        menuName = InitializationStepsPath + nameof(RepositoriesStep)
    )]
    internal sealed class RepositoriesStep : StepBase
    {
        [Inject] private IEnumerable<IRepository> _defaultRepositories;
        [Inject] private IObjectResolver _objectResolver;

        protected override async UniTask ExecuteInternal(CancellationToken token)
        {
            var tasks = new List<UniTask>();

            foreach (var defaultConfig in _defaultRepositories)
            {
                _objectResolver.Inject(defaultConfig);

                tasks.Add(defaultConfig.InitAsync(token));
            }

            await UniTask.WhenAll(tasks);
        }
    }
}