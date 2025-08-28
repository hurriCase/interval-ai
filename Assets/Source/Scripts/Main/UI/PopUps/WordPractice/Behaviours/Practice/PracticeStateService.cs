using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice
{
    internal sealed class PracticeStateService : IPracticeStateService
    {
        public ReadOnlyReactiveProperty<PracticeState> CurrentState => _currentState;
        private readonly ReactiveProperty<PracticeState> _currentState = new(PracticeState.NewWords);
        private readonly ICurrentWordsService _currentWordsService;

        public PracticeStateService(ICurrentWordsService currentWordsService)
        {
            _currentWordsService = currentWordsService;
        }

        public void SetState(PracticeState state) => _currentState.Value = state;

        public void UpdatePracticeState()
        {
            _currentWordsService.UpdateCurrentWords();

            var hasNewWords = _currentWordsService.HasWordByState(PracticeState.NewWords);
            var hasReviewWords = _currentWordsService.HasWordByState(PracticeState.Review);

            if (hasNewWords is false && hasReviewWords)
            {
                SetState(PracticeState.Review);
                return;
            }

            SetState(PracticeState.NewWords);
        }
    }
}