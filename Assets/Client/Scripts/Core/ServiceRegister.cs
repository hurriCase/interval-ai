using Client.Scripts.Core.AI;
using Client.Scripts.Core.Audio;
using Client.Scripts.Core.SignIn;
using Client.Scripts.DB.Data;
using Client.Scripts.DB.DataRepositories.Cloud;
using Client.Scripts.DB.DataRepositories.Offline;
using Client.Scripts.DB.Entities.Base.Validation;
using Client.Scripts.DB.Entities.EntityController;
using DependencyInjection.Runtime;
using DependencyInjection.Runtime.InjectionBase.Service;

namespace Client.Scripts.Core
{
    internal sealed class ServiceRegister : ServiceRegisterBase
    {
        protected override void ConfigureStaticServices()
        {
            DIContainer.RegisterSingleton<ICloudRepository, FireBaseRepository>();
            DIContainer.RegisterSingleton<IOfflineRepository, PlayerPrefsRepository>();
            DIContainer.RegisterSingleton<IEntityController, EntityController>();
            DIContainer.RegisterSingleton<IUserDataController, UserDataController>();
            DIContainer.RegisterSingleton<IAIController, GeminiAPI>();
            DIContainer.RegisterSingleton<IEntityValidationController, EntityValidationController>();
#if UNITY_EDITOR
            DIContainer.RegisterSingleton<IAuthorizationController, UnitySignInController>();
#else
            DIContainer.RegisterSingleton<IAuthorizationController, GoogleSignInController>();
#endif
        }

        protected override void ConfigureRuntimeServices()
        {
            DIContainer.RegisterSingleton<IAudioController>(AudioController.Instance);
        }
    }
}