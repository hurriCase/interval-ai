using System;
using System.Collections.Generic;
using Client.Scripts.Core.StartUp.Steps;
using UnityEngine;

namespace Client.Scripts.Core.StartUp
{
    internal sealed class StartUpController : MonoBehaviour
    {
        internal static bool IsInited { get; private set; }
        internal static event Action OnInitializationCompleted;

        private static readonly IReadOnlyList<Type> _stepTypes = new List<Type>
        {
            typeof(AssetLoaderStep),
            typeof(DIStep),
            typeof(DataStep),
            typeof(AIStep)
        };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void InitializeApplication()
        {
            try
            {
                if (IsInited)
                    return;

                for (var i = 0; i < _stepTypes.Count; i++)
                {
                    var step = StepFactory.CreateStep(_stepTypes[i]);
                    step.OnStepCompleted += LogStepCompletion;
                    await step.Execute(i);
                }

                IsInited = true;

                OnInitializationCompleted?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"[StartUpController::InitializeApplication] Initialization failed, with error: {e.Message}");
                throw;
            }
        }

        private static void LogStepCompletion(int step, string stepName)
        {
            Debug.Log($"[StartUpController::LogStepCompletion] Step {step} completed: {stepName}");
        }

        private void OnDestroy()
        {
            OnInitializationCompleted = null;
        }
    }
}