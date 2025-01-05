using System;
using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;

namespace Client.Scripts.Core.StartUp.Steps
{
    internal sealed class AIStep : Injectable, IStep
    {
        [Inject] private IAIController _aiController;

        public event Action<int, string> OnCompleted;

        public async Task Execute(int step)
        {
            try
            {
                await _aiController.InitAsync();

                OnCompleted?.Invoke(step, GetType().Name);
            }
            catch (Exception e)
            {
                Debug.LogError($"[DBStep::Execute] {GetType().Name} step initialization is failed: {e.Message}");
            }
        }
    }
}