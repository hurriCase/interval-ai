using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete.CompleteState
{
    internal sealed class CompleteServiceFactory : StateFactoryBase<PracticeState, ICompleteStateService>,
        ICompleteServiceFactory
    {
        private readonly ICurrentWordFactory _currentWordFactory;
        private readonly IProgressRepository _progressRepository;
        private readonly IWordsTimerService _wordsTimerService;

        internal CompleteServiceFactory(
            ICurrentWordFactory currentWordFactory,
            IProgressRepository progressRepository,
            IWordsTimerService wordsTimerService)
        {
            _currentWordFactory = currentWordFactory;
            _progressRepository = progressRepository;
            _wordsTimerService = wordsTimerService;
        }

        protected override ICompleteStateService CreateService(PracticeState practiceState)
        {
            var currentWordService = _currentWordFactory.GetOrCreate(practiceState);
            return new CompleteStateService(
                currentWordService,
                _progressRepository,
                _wordsTimerService,
                practiceState);
        }
    }
}