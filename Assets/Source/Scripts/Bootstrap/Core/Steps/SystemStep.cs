using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Source.Scripts.Bootstrap.Core.Steps
{
    [CreateAssetMenu(
        fileName = nameof(SystemStep),
        menuName = InitializationStepsPath + nameof(SystemStep)
    )]
    internal sealed class SystemStep : StepBase
    {
        protected override UniTask ExecuteInternal(CancellationToken token)
        {
            Application.targetFrameRate = 60;

            return UniTask.CompletedTask;
        }
    }
}