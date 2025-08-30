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
        private IReadOnlyList<TLoadable> _entitiesToLoad;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(IReadOnlyList<TLoadable> entitiesToLoad, IObjectResolver objectResolver)
        {
            _entitiesToLoad = entitiesToLoad;
            _objectResolver = objectResolver;
        }

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