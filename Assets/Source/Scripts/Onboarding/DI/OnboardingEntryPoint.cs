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
        private readonly IDefaultDataDatabase _defaultOnboardingDatabase;
        private readonly ICurrentWordsService _currentWordsService;
        private readonly IWindowsController _windowsController;
        private readonly IObjectResolver _objectResolver;

        internal OnboardingEntryPoint(
            IDefaultDataDatabase defaultOnboardingDatabase,
            ICurrentWordsService currentWordsService,
            IWindowsController windowsController,
            IObjectResolver objectResolver)
        {
            _defaultOnboardingDatabase = defaultOnboardingDatabase;
            _currentWordsService = currentWordsService;
            _windowsController = windowsController;
            _objectResolver = objectResolver;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await _windowsController.InitAsync(cancellationToken);

            _objectResolver.Inject(_defaultOnboardingDatabase);
            await _defaultOnboardingDatabase.InitAsync(cancellationToken);

            _currentWordsService.UpdateCurrentWords();
        }
    }
}