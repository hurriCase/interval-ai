using System.Threading;
using CustomUtils.Runtime.Localization;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.DI.Repositories.Progress.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Core.DI.StartUp.Steps
{
    [CreateAssetMenu(
        fileName = nameof(TestsStep),
        menuName = ResourcePaths.InitializationStepsPath + nameof(TestsStep)
    )]
    internal sealed class TestsStep : StepBase
    {
        [Inject] private ITestDataFactory _testDataFactory;

        [SerializeField] private SystemLanguage _testLanguage;

        protected override UniTask ExecuteInternal(CancellationToken token)
        {
#if IS_DEBUG && UNITY_EDITOR
            _testDataFactory.CreateFakeProgress();

            LocalizationController.Language.Value = _testLanguage;
#endif

            return UniTask.CompletedTask;
        }
    }
}