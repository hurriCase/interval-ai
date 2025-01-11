using System;
using System.Threading.Tasks;
using Client.Scripts.Core.AiController;
using Client.Scripts.DB.DBControllers;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;

namespace Client.Scripts.Core.StartUp.Steps
{
    internal sealed class DIStep : IStep
    {
        public event Action<int, string> OnCompleted;

        public Task Execute(int step)
        {
            try
            {
                RegisterServices();

                OnCompleted?.Invoke(step, GetType().Name);
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Debug.LogError($"[DIStep::Execute] {GetType().Name} step initialization is failed: {e.Message}");
            }

            return Task.CompletedTask;
        }

        private void RegisterServices()
        {
            DIContainer.RegisterSingleton<IAudioController>(AudioController.Instance);
            DIContainer.Register<IDBController>(() => new FireBaseDB());
            DIContainer.Register<IEntityController>(() => new EntityController());
            DIContainer.Register<IUserDataController>(() => new UserDataController());
            DIContainer.Register<IAIController>(() => new GeminiAPI());
#if UNITY_EDITOR
            DIContainer.Register<IAuthorizationController>(() => new UnitySignInController());
#else
            DIContainer.Register<IAuthorizationController>(() => new GoogleSignInController());
#endif
        }
    }
}