using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.AI;
using Source.Scripts.Core.Loader;
using Source.Scripts.UI.Windows;
using Source.Scripts.UI.Windows.Base;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.DI
{
    internal sealed class MainEntryPoint : IAsyncStartable
    {
        [Inject] private IWindowsController _windowsController;
        [Inject] private IAddressablesLoader _addressablesLoader;
        [Inject] private IObjectResolver _objectResolver;

        [Inject] private IMenuBehaviour _menuBehaviour;

        [Inject] private IAIController _aiController;

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await _windowsController.InitAsync(_objectResolver, _addressablesLoader, cancellationToken);

            _menuBehaviour.Init(cancellationToken);
        }
    }
}