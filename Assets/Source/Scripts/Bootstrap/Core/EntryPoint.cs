using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Scenes.Base;
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

        private readonly List<StepBase> _stepsList;

        internal EntryPoint(
            ISceneTransitionController sceneTransitionController,
            IStatisticsRepository statisticsRepository,
            ISceneReferences sceneReferences,
            IObjectResolver objectResolver,
            List<StepBase> stepsList)
        {
            _stepsList = stepsList;
            _objectResolver = objectResolver;
            _sceneTransitionController = sceneTransitionController;
            _statisticsRepository = statisticsRepository;
            _sceneReferences = sceneReferences;
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            await InitSteps(token);

            _statisticsRepository.LoginHistory.Value[DateTime.Now] = true;

            var sceneAddressToLoad = _statisticsRepository.IsCompleteOnboarding.Value
                ? _sceneReferences.MainMenuScene.Address
                : _sceneReferences.Onboarding.Address;

            _sceneTransitionController.StartTransition(_sceneReferences.Splash.Address, sceneAddressToLoad).Forget();
        }

        private async UniTask InitSteps(CancellationToken cancellationToken)
        {
            try
            {
                for (var i = 0; i < _stepsList.Count; i++)
                {
                    _stepsList[i].OnStepCompleted
                        .Subscribe(this, static (stepData, self) => self.LogStepCompletion(stepData))
                        .RegisterTo(cancellationToken);

                    _objectResolver.Inject(_stepsList[i]);
                    await _stepsList[i].Execute(i, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private void LogStepCompletion(StepData stepData)
        {
            Debug.Log($"[StartUpService::LogStepCompletion] Step {stepData.Step} completed: {stepData.StepName}");
        }
    }
}