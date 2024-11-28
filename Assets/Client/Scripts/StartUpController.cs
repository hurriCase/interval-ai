using Client.Scripts.Database.Base;
using Client.Scripts.Database.Controllers;
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
            DIContainer.Register<IVocabularyEntityController>(() => new VocabularyEntityController());
            DIContainer.Register<IUserEntityController>(() => new UserEntityController());

            var audioController = DIContainer.Resolve<IAudioController>();
            audioController.PlayMusic();

            var dbController = DIContainer.Resolve<IDBController>();
            await dbController.Init();

            var vocabularyController = DIContainer.Resolve<IVocabularyEntityController>();
            await vocabularyController.Init();

            var userController = DIContainer.Resolve<IUserEntityController>();
            await userController.Init();
        }
    }
}