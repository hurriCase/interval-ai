using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Scenes;
using Source.Scripts.Data.Repositories.User;
using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace Source.Scripts.Core.StartUp
{
    internal sealed class CoreEntryPoint : IAsyncStartable
    {
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private ISceneLoader _sceneLoader;
        [Inject] private IUserRepository _userRepository;

        private readonly List<StepBase> _stepsList;

        internal CoreEntryPoint(List<StepBase> stepsList)
        {
            _stepsList = stepsList;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await InitSteps(cancellation);

            var addressToLoad = _userRepository.IsCompleteOnboarding.Value
                ? SceneReferences.Instance.MainMenuScene.Address
                : SceneReferences.Instance.Onboarding.Address;

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