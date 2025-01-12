using System;
using System.Threading.Tasks;
using Client.Scripts.Core.AI;
using Client.Scripts.Core.Audio;
using Client.Scripts.Core.SignIn;
using Client.Scripts.DB.Data;
using Client.Scripts.DB.DataRepositories.Cloud;
using Client.Scripts.DB.DataRepositories.Offline;
using Client.Scripts.DB.Entities.Base.Validation;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.Patterns.DI.Base;
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
                RegisterServices();

                OnStepCompleted?.Invoke(step, GetType().Name);
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
            DIContainer.Register<ICloudRepository>(() => new FireBaseRepository());
            DIContainer.Register<IOfflineRepository>(() => new PlayerPrefsRepository());
            DIContainer.Register<IEntityController>(() => new EntityController());
            DIContainer.Register<IUserDataController>(() => new UserDataController());
            DIContainer.Register<IAIController>(() => new GeminiAPI());
            DIContainer.Register<IEntityValidationController>(() => new EntityValidationController());
#if UNITY_EDITOR
            DIContainer.Register<IAuthorizationController>(() => new UnitySignInController());
#else
            DIContainer.Register<IAuthorizationController>(() => new GoogleSignInController());
#endif
        }
    }
}