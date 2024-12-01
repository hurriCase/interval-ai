using System;
using System.Threading.Tasks;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;

namespace Client.Scripts.Steps
{
    internal sealed class SceneContextStep : Injectable, IStep
    {
        [Inject] private IAudioController _audioController;

        public event Action<int, string> OnCompleted;

        public Task Execute(int step)
        {
            _audioController.PlayMusic();

            OnCompleted?.Invoke(step, GetType().Name);
            return Task.CompletedTask;
        }
    }
}