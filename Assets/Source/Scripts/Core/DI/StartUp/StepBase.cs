using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Source.Scripts.Core.DI.StartUp
{
    internal abstract class StepBase : ScriptableObject
    {
        private readonly Subject<StepData> _stepCompletedSubject = new();
        internal Observable<StepData> OnStepCompleted => _stepCompletedSubject.AsObservable();

        internal virtual async UniTask Execute(int step, CancellationToken token)
        {
            try
            {
                await ExecuteInternal(token);
                _stepCompletedSubject.OnNext(new StepData { Step = step, StepName = GetType().Name });
            }
            catch (Exception e)
            {
                Debug.LogError($"[{GetType().Name}::Execute] Step initialization failed: {e.Message}");
            }
        }

        protected abstract UniTask ExecuteInternal(CancellationToken token);
    }
}