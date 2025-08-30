using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.UI.Windows.Menu;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.DI
{
    internal sealed class MainEntryPoint : IAsyncStartable
    {
        private IWindowsController _windowsController;
        private IMenuBehaviour _menuBehaviour;

        [Inject]
        internal void Inject(IWindowsController windowsController, IMenuBehaviour menuBehaviour)
        {
            _windowsController = windowsController;
            _menuBehaviour = menuBehaviour;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await _windowsController.InitAsync(cancellationToken);

            _menuBehaviour.Init(cancellationToken);
        }
    }
}