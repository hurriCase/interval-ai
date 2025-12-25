using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Scenes.Base;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Statistics;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Bootstrap.Core
{
    internal sealed class EntryPoint : IAsyncStartable
    {
        private readonly ISceneTransitionController _sceneTransitionController;
        private readonly IStatisticsRepository _statisticsRepository;
        private readonly ISceneReferences _sceneReferences;
        private readonly IObjectResolver _objectResolver;
        private readonly List<StepBase> _steps;

        internal EntryPoint(
            ISceneTransitionController sceneTransitionController,
            IStatisticsRepository statisticsRepository,
            ISceneReferences sceneReferences,
            IObjectResolver objectResolver,
            List<StepBase> steps)
        {
            _sceneTransitionController = sceneTransitionController;
            _statisticsRepository = statisticsRepository;
            _sceneReferences = sceneReferences;
            _objectResolver = objectResolver;
            _steps = steps;
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            await InitSteps(token);

            _statisticsRepository.MarkNewLogin();

            var sceneAddressToLoad = _statisticsRepository.IsCompleteOnboarding.Value
                ? _sceneReferences.Main.Address
                : _sceneReferences.Onboarding.Address;

            _sceneTransitionController.StartTransition(_sceneReferences.Splash.Address, sceneAddressToLoad).Forget();
        }

        private async UniTask InitSteps(CancellationToken cancellationToken)
        {
            try
            {
                for (var i = 0; i < _steps.Count; i++)
                {
                    _steps[i].OnStepCompletedObservable
                        .Subscribe(this, static (stepData, self) => self.LogStepCompletion(stepData))
                        .RegisterTo(cancellationToken);

                    _objectResolver.Inject(_steps[i]);
                    await _steps[i].Execute(i, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void LogStepCompletion(StepData stepData)
        {
            var message = ZString.Format("[EntryPoint::LogStepCompletion] Step {0} completed: {1}",
                stepData.Step, stepData.StepName);

            Debug.Log(message);
        }
    }
}