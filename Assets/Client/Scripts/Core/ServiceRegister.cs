using Client.Scripts.Core.AI;
using Client.Scripts.Core.Audio;
using Client.Scripts.Core.SignIn;
using Client.Scripts.DB.Data;
using Client.Scripts.DB.DataRepositories.Cloud;
using Client.Scripts.DB.DataRepositories.Offline;
using Client.Scripts.DB.Entities.Base.Validation;
using Client.Scripts.DB.Entities.EntityController;
using Client.Scripts.Patterns.DI;

namespace Client.Scripts.Core
{
    internal static class ServiceRegister
    {
        internal static void RegisterSceneServices()
        {
            DIContainer.RegisterSingleton<IAudioController>(AudioController.Instance);
        }

        internal static void RegisterRegularServices()
        {
            DIContainer.RegisterSingleton<ICloudRepository>(FireBaseRepository.Instance);
            DIContainer.RegisterSingleton<IOfflineRepository>(PlayerPrefsRepository.Instance);
            DIContainer.RegisterSingleton<IEntityController>(EntityController.Instance);
            DIContainer.RegisterSingleton<IUserDataController>(UserDataController.Instance);
            DIContainer.RegisterSingleton<IAIController>(GeminiAPI.Instance);
            DIContainer.RegisterSingleton<IEntityValidationController>(EntityValidationController.Instance);
#if UNITY_EDITOR
            DIContainer.RegisterSingleton<IAuthorizationController>(UnitySignInController.Instance);
#else
            DIContainer.RegisterSingleton<IAuthorizationController>(GoogleSignInController.Instance);
#endif
        }
    }
}