using System.Threading;
using CustomUtils.Runtime.Localization;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base.Tests.Base;
using Source.Scripts.Core.Repositories.Statistics;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Bootstrap.Core.Steps
{
    [CreateAssetMenu(
        fileName = nameof(TestStep),
        menuName = InitializationStepsPath + nameof(TestStep)
    )]
    internal sealed class TestStep : StepBase
    {
        private IStatisticsRepository _statisticsRepository;
        private ITestDataFactory _testDataFactory;
        private ITestConfig _testConfig;

        [Inject]
        internal void Inject(
            IStatisticsRepository statisticsRepository,
            ITestDataFactory testDataFactory,
            ITestConfig testConfig)
        {
            _statisticsRepository = statisticsRepository;
            _testDataFactory = testDataFactory;
            _testConfig = testConfig;
        }

        protected override UniTask ExecuteInternal(CancellationToken token)
        {
#if IS_DEBUG && UNITY_EDITOR
            _testDataFactory.CreateFakeProgress();

            if (_testConfig.UseTestLanguage)
                LocalizationController.Language.Value = _testConfig.TestLanguage;

            if (_testConfig.IsSkipOnboarding)
                _statisticsRepository.IsCompleteOnboarding.Value = true;
#endif

            return UniTask.CompletedTask;
        }
    }
}