using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.UI.Windows;
using Source.Scripts.UI.Windows.Base;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.Source.Scripts.Main.DI
{
    //TODO:<Dmitriy.Sukharev> probably, it's a wrong place for this.
    //Yes, it fully belongs to UI, but it's just for now, and it's more about main scene context, not UI
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