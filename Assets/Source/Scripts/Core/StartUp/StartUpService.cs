using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Scenes;
using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace Source.Scripts.Core.StartUp
{
    internal sealed class StartUpService : IAsyncStartable
    {
        [Inject] private ISceneLoader _sceneLoader;

        private readonly List<StepBase> _stepsList;

        internal StartUpService(List<StepBase> stepsList)
        {
            _stepsList = stepsList;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await InitSteps(cancellation);

            _sceneLoader.LoadSceneAsync(SceneReferences.Instance.MainMenuScene.Address, cancellation).Forget();
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