using Source.Scripts.Core.DI;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;

namespace Source.Scripts.Core.Repositories.Words.Advance
{
    internal sealed class WordAdvanceFactory : StateFactoryBase<PracticeState, IWordAdvanceService>,
        IWordAdvanceFactory
    {
        private readonly ICurrentWordFactory _currentWordFactory;
        private readonly IProgressRepository _progressRepository;
        private readonly IWordsTimerService _wordsTimerService;
        private readonly IWordStateMutator _wordStateMutator;

        internal WordAdvanceFactory(
            ICurrentWordFactory currentWordFactory,
            IProgressRepository progressRepository,
            IWordsTimerService wordsTimerService,
            IWordStateMutator wordStateMutator)
        {
            _currentWordFactory = currentWordFactory;
            _progressRepository = progressRepository;
            _wordsTimerService = wordsTimerService;
            _wordStateMutator = wordStateMutator;
        }

        protected override IWordAdvanceService CreateService(PracticeState practiceState)
        {
            var currentWordService = _currentWordFactory.GetOrCreate(practiceState);
            return new WordAdvanceService(
                currentWordService,
                _progressRepository,
                _wordsTimerService,
                _wordStateMutator);
        }
    }
}