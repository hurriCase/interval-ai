using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.DI.Repositories.Statistics;
using Source.Scripts.Core.Scenes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Core.DI.StartUp
{
    internal sealed class CoreEntryPoint : IAsyncStartable
    {
        private readonly IObjectResolver _objectResolver;
        private readonly ISceneLoader _sceneLoader;
        private readonly ISceneReferences _sceneReferences;

        private readonly List<StepBase> _stepsList;

        internal CoreEntryPoint(
            List<StepBase> stepsList,
            IObjectResolver objectResolver,
            ISceneLoader sceneLoader,
            ISceneReferences sceneReferences)
        {
            _stepsList = stepsList;
            _objectResolver = objectResolver;
            _sceneLoader = sceneLoader;
            _sceneReferences = sceneReferences;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await InitSteps(cancellation);

            var statisticsRepository = _objectResolver.Resolve<IStatisticsRepository>();

            statisticsRepository.LoginHistory.Value[DateTime.Now] = true;

            var addressToLoad = statisticsRepository.IsCompleteOnboarding.Value
                ? _sceneReferences.MainMenuScene.Address
                : _sceneReferences.Onboarding.Address;

            _sceneLoader.LoadSceneAsync(addressToLoad, cancellation).Forget();
        }

        private async UniTask InitSteps(CancellationToken token)
        {
            try
            {
                for (var i = 0; i < _stepsList.Count; i++)
                {
                    _stepsList[i].OnStepCompleted
                        .Subscribe(static stepData => Debug.Log("[StartUpService::LogStepCompletion] " +
                                                                $"Step {stepData.Step} completed: {stepData.StepName}"))
                        .RegisterTo(token);

                    _objectResolver.Inject(_stepsList[i]);
                    await _stepsList[i].Execute(i, token);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("[StartUpService::InitSteps] " +
                               $"Initialization failed, with error: {e.Message}");
            }
        }
    }
}