using System.Threading;
using CustomUtils.Runtime.Scenes.Base;
using Cysharp.Threading.Tasks;
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
        private IMenuBehaviour _menuBehaviour;

        [Inject]
        internal void Inject(
            ISceneTransitionController sceneTransitionController,
            IWindowsController windowsController,
            IMenuBehaviour menuBehaviour)
        {
            _sceneTransitionController = sceneTransitionController;
            _windowsController = windowsController;
            _menuBehaviour = menuBehaviour;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await _windowsController.InitAsync(cancellationToken);

            _menuBehaviour.Init(cancellationToken);

            _sceneTransitionController.EndTransition();
        }
    }
}