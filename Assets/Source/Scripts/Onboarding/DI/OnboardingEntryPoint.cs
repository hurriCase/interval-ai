using System.Threading;
using CustomUtils.Runtime.Scenes.Base;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base.DefaultConfig;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using Source.Scripts.Onboarding.UI.Base;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.DI
{
    internal sealed class OnboardingEntryPoint : IAsyncStartable
    {
        private readonly ISceneTransitionController _sceneTransitionController;
        private readonly IDefaultDataDatabase _defaultOnboardingDatabase;
        private readonly ICurrentWordService _currentWordService;
        private readonly IWindowsController _windowsController;
        private readonly IObjectResolver _objectResolver;

        internal OnboardingEntryPoint(
            ISceneTransitionController sceneTransitionController,
            IDefaultDataDatabase defaultOnboardingDatabase,
            ICurrentWordService currentWordService,
            IWindowsController windowsController,
            IObjectResolver objectResolver)
        {
            _sceneTransitionController = sceneTransitionController;
            _defaultOnboardingDatabase = defaultOnboardingDatabase;
            _currentWordService = currentWordService;
            _windowsController = windowsController;
            _objectResolver = objectResolver;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            await _windowsController.InitAsync(cancellationToken);

            _objectResolver.Inject(_defaultOnboardingDatabase);
            await _defaultOnboardingDatabase.InitAsync(cancellationToken);

            _currentWordService.UpdateCurrentWord();

            _sceneTransitionController.EndTransition();
        }
    }
}