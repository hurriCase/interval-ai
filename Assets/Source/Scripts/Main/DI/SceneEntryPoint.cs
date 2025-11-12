using System.Threading;
using CustomUtils.Runtime.Scenes.Base;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.GenerativeLanguage;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Windows.Menu;
using VContainer.Unity;

namespace Source.Scripts.Main.DI
{
    internal sealed class SceneEntryPoint : IAsyncStartable
    {
        private readonly ISceneTransitionController _sceneTransitionController;
        private readonly IWindowsController _windowsController;
        private readonly IGenerativeLanguage _generativeLanguage;
        private readonly IMenuBehaviour _menuBehaviour;

        internal SceneEntryPoint(
            ISceneTransitionController sceneTransitionController,
            IWindowsController windowsController,
            IGenerativeLanguage generativeLanguage,
            IMenuBehaviour menuBehaviour)
        {
            _sceneTransitionController = sceneTransitionController;
            _windowsController = windowsController;
            _generativeLanguage = generativeLanguage;
            _menuBehaviour = menuBehaviour;
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            await _generativeLanguage.InitAsync(token);
            await _windowsController.InitAsync(token);

            _menuBehaviour.Init();

            _sceneTransitionController.EndTransition();
        }
    }
}