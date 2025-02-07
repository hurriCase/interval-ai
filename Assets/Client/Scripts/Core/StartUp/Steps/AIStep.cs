using System;
using System.Threading.Tasks;
using Client.Scripts.Core.AI;
using DependencyInjection.Runtime.InjectableMarkers;
using DependencyInjection.Runtime.InjectionBase;
using UnityEngine;

namespace Client.Scripts.Core.StartUp.Steps
{
    internal sealed class AIStep : Injectable, IStep
    {
        [Inject] private IAIController _aiController;

        public event Action<int, string> OnStepCompleted;

        public async Task Execute(int step)
        {
            try
            {
                await _aiController.InitAsync();

                OnStepCompleted?.Invoke(step, GetType().Name);
            }
            catch (Exception e)
            {
                Debug.LogError($"[DBStep::Execute] {GetType().Name} step initialization is failed: {e.Message}");
            }
        }
    }
}