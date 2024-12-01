using System;
using System.Threading.Tasks;
using Client.Scripts.DB;
using Client.Scripts.DB.Base;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;

namespace Client.Scripts.Core.StartUp.Steps
{
    internal sealed class DIStep : IStep
    {
        public event Action<int, string> OnCompleted;

        public Task Execute(int step)
        {
            RegisterServices();

            OnCompleted?.Invoke(step, GetType().Name);
            return Task.CompletedTask;
        }

        private void RegisterServices()
        {
            DIContainer.RegisterSingleton<IAudioController>(AudioController.Instance);
            DIContainer.Register<IDBController>(() => new DBController());
            DIContainer.Register<IEntityController>(() => new EntityController());
        }
    }
}