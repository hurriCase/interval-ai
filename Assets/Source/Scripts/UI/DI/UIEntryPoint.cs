using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.UI.Windows;
using Source.Scripts.UI.Windows.Base;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.UI.DI
{
    public sealed class UIEntryPoint : IAsyncStartable
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