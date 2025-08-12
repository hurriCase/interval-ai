using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.AI;
using Source.Scripts.Core.Loader;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.Menu;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.DI
{
    internal sealed class MainEntryPoint : IAsyncStartable
    {
        [Inject] private IWindowsController _windowsController;
        [Inject] private IMenuBehaviour _menuBehaviour;

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await _windowsController.InitAsync(cancellationToken);

            _menuBehaviour.Init(cancellationToken);
        }
    }
}