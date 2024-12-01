using System;
using System.Threading.Tasks;
using Client.Scripts.DB;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;

namespace Client.Scripts.Core.StartUp.Steps
{
    internal sealed class DBStep : Injectable, IStep
    {
        [Inject] private IDBController _dbController;
        [Inject] private IEntityController _entityController;

        public event Action<int, string> OnCompleted;

        public async Task Execute(int step)
        {
            await _dbController.InitAsync();
            await _entityController.InitAsync();

            OnCompleted?.Invoke(step, GetType().Name);
        }
    }
}