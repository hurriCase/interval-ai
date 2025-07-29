using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.AI;
using Source.Scripts.UI.Windows;
using Source.Scripts.UI.Windows.Base;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Main.Source.Scripts.Main.DI
{
    internal sealed class MainEntryPoint : IAsyncStartable
    {
        [Inject] private IWindowsController _windowsController;
        [Inject] private IMenuBehaviour _menuBehaviour;

        [Inject] private IAIController _aiController;

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await _windowsController.InitAsync(cancellationToken);

            _menuBehaviour.Init(cancellationToken);
        }
    }
}