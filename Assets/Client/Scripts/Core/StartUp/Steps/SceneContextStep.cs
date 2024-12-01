using System;
using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;

namespace Client.Scripts.Core.StartUp.Steps
{
    internal sealed class SceneContextStep : Injectable, IStep
    {
        [Inject] private IAudioController _audioController;

        public event Action<int, string> OnCompleted;

        public Task Execute(int step)
        {
            try
            {
                _audioController.PlayMusic();

                OnCompleted?.Invoke(step, GetType().Name);
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