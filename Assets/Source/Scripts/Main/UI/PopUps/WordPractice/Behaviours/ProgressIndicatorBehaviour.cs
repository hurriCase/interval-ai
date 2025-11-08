using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Advance;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts;
using Source.Scripts.Main.UI.PopUps.WordControl;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class ProgressIndicatorBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _learnedText;
        [SerializeField] private ThemeButton _previousCardButton;
        [SerializeField] private ThemeButton _moreButton;

        [SerializeField] private PluralLocalization _learnedCounts;

        private ICurrentWordFactory _currentWordFactory;
        private IWordAdvanceFactory _wordAdvanceFactory;
        private IProgressRepository _progressRepository;
        private IWindowsController _windowsController;

        private PracticeState _currentPracticeState;

        [Inject]
        internal void Inject(
            ICurrentWordFactory currentWordFactory,
            IWordAdvanceFactory wordAdvanceFactory,
            IProgressRepository progressRepository,
            IWindowsController windowsController)
        {
            _currentWordFactory = currentWordFactory;
            _wordAdvanceFactory = wordAdvanceFactory;
            _progressRepository = progressRepository;
            _windowsController = windowsController;
        }

        public void Init(PracticeState practiceState)
        {
            _currentPracticeState = practiceState;

            var wordAdvanceService = _wordAdvanceFactory.GetOrCreate(practiceState);
            wordAdvanceService.CanUndo
                .SubscribeUntilDestroy(this, static (canUndo, self) => self._previousCardButton.SetActive(canUndo));

            _progressRepository.LearnedWordCounts[practiceState].SubscribePluralToText(_learnedCounts, _learnedText);

            _previousCardButton.OnClickAsObservable()
                .Subscribe(wordAdvanceService.UndoCommand, static (unit, undo) => undo.Execute(unit))
                .RegisterTo(destroyCancellationToken);

            _moreButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.OpenWordControlPopUp());
        }

        private void OpenWordControlPopUp()
        {
            var wordControlPopUp = _windowsController.OpenPopUp<WordControlPopUp>();
            var currentWordService = _currentWordFactory.GetOrCreate(_currentPracticeState);
            var currentWord = currentWordService.CurrentWord.CurrentValue;
            wordControlPopUp.SetParameters(currentWord);
        }
    }
}