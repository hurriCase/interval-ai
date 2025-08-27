using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Others;
using VContainer;

namespace Source.Scripts.Bootstrap.Core.Steps.Load
{
    internal abstract class LoadStepBase<TLoadable> : StepBase
        where TLoadable : class, ILoadable
    {
        [Inject] private IReadOnlyList<TLoadable> _entitiesToLoad;
        [Inject] private IObjectResolver _objectResolver;

        protected override async UniTask ExecuteInternal(CancellationToken token)
        {
            var entities = _entitiesToLoad.ToList();
            var tasks = new UniTask[entities.Count];

            for (var i = 0; i < entities.Count; i++)
            {
                var loadable = entities[i];
                _objectResolver.Inject(loadable);
                tasks[i] = entities[i].InitAsync(token);
            }

            await UniTask.WhenAll(tasks);
        }
    }
}