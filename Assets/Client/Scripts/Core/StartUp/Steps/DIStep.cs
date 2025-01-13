using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Client.Scripts.Core.StartUp.Steps
{
    internal sealed class DIStep : IStep
    {
        public event Action<int, string> OnStepCompleted;

        public Task Execute(int step)
        {
            try
            {
                ServiceRegister.RegisterSceneServices();
                ServiceRegister.RegisterRegularServices();

                OnStepCompleted?.Invoke(step, GetType().Name);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError($"[DIStep::Execute] {GetType().Name} step initialization is failed: {e.Message}");
            }

            return Task.CompletedTask;
        }
    }
}