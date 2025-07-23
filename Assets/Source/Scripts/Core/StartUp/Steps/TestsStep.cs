using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Data.Repositories.Progress.Tests;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Core.StartUp.Steps
{
    [CreateAssetMenu(fileName = nameof(TestsStep), menuName = ResourcePaths.StepsPath + nameof(TestsStep))]
    internal sealed class TestsStep : StepBase
    {
        [Inject] private ITestDataFactory _testDataFactory;

        protected override UniTask ExecuteInternal(CancellationToken token)
        {
#if IS_DEBUG && UNITY_EDITOR
            _testDataFactory.CreateFakeProgress();
#endif

            return UniTask.CompletedTask;
        }
    }
}