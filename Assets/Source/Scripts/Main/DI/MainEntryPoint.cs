using System.Threading;
using CustomUtils.Runtime.Scenes.Base;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.AI;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Windows.Menu;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.DI
{
    internal sealed class MainEntryPoint : IAsyncStartable
    {
        private ISceneTransitionController _sceneTransitionController;
        private IWindowsController _windowsController;
        private IAITextController _aiTextController;
        private IMenuBehaviour _menuBehaviour;

        [Inject]
        internal void Inject(
            ISceneTransitionController sceneTransitionController,
            IWindowsController windowsController,
            IAITextController aiTextController,
            IMenuBehaviour menuBehaviour)
        {
            _sceneTransitionController = sceneTransitionController;
            _windowsController = windowsController;
            _aiTextController = aiTextController;
            _menuBehaviour = menuBehaviour;
        }

        public async UniTask StartAsync(CancellationToken token)
        {
            await _aiTextController.InitAsync(token);
            await _windowsController.InitAsync(token);

            _menuBehaviour.Init();

            _sceneTransitionController.EndTransition();
        }
    }
}