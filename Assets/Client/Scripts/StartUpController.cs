using Client.Scripts.Database;
using Client.Scripts.Database.Base;
using Client.Scripts.Patterns.DI.Base;
using Client.Scripts.Patterns.DI.Services;
using UnityEngine;

namespace Client.Scripts
{
    internal sealed class StartUpController : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void OnBeforeSceneLoadRuntimeMethod()
        {
            DIContainer.RegisterSingleton<IAudioController>(AudioController.Instance);
            DIContainer.Register<IDBController>(() => new DBController());
            DIContainer.Register<IEntityController>(() => new EntityController());

            var audioController = DIContainer.Resolve<IAudioController>();
            audioController.PlayMusic();

            var dbController = DIContainer.Resolve<IDBController>();
            await dbController.Init();

            var entityController = DIContainer.Resolve<IEntityController>();
            await entityController.Init();
        }
    }
}