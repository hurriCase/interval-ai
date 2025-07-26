using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.UI.Windows.Base;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.DI
{
    [Preserve]
    internal sealed class OnboardingEntryPoint : IAsyncStartable
    {
        [Inject] private IWindowsController _windowsController;

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await _windowsController.InitAsync(cancellationToken);
        }
    }
}