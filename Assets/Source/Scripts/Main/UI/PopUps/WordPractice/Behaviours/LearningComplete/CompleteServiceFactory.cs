using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal sealed class CompleteServiceFactory : StateFactoryBase<PracticeState, ICompleteStateService>,
        ICompleteServiceFactory
    {
        private readonly ICurrentWordsService _currentWordsService;
        private readonly IProgressRepository _progressRepository;
        private readonly IWordsTimerService _wordsTimerService;

        internal CompleteServiceFactory(
            ICurrentWordsService currentWordsService,
            IProgressRepository progressRepository,
            IWordsTimerService wordsTimerService)
        {
            _currentWordsService = currentWordsService;
            _progressRepository = progressRepository;
            _wordsTimerService = wordsTimerService;
        }

        protected override ICompleteStateService CreateService(PracticeState practiceState) =>
            new CompleteStateService(_currentWordsService, _progressRepository, _wordsTimerService, practiceState);
    }
}