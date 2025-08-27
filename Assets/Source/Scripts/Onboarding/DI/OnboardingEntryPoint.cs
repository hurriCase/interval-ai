using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base.DefaultConfig;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Onboarding.UI.Base;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.DI
{
    internal sealed class OnboardingEntryPoint : IAsyncStartable
    {
        [Inject] private IWindowsController _windowsController;
        [Inject] private ICurrentWordsService _currentWordsService;
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private IDefaultDataDatabase _defaultOnboardingDatabase;

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await _windowsController.InitAsync(cancellationToken);

            _objectResolver.Inject(_defaultOnboardingDatabase);
            await _defaultOnboardingDatabase.InitAsync(cancellationToken);

            _currentWordsService.UpdateCurrentWords();
        }
    }
}