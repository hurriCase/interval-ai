using CustomUtils.Runtime.Extensions;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.WordControl;
using Source.Scripts.UI.Components.Button;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class ProgressIndicatorBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _learnedText;
        [SerializeField] private ButtonComponent _previousCardButton;
        [SerializeField] private ButtonComponent _moreButton;

        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private ICurrentWordsService _currentWordsService;
        [Inject] private IWordAdvanceService _wordAdvanceService;
        [Inject] private IProgressRepository _progressRepository;
        [Inject] private IWindowsController _windowsController;

        private PracticeState _currentPracticeState;

        public void Init(PracticeState practiceState)
        {
            _currentPracticeState = practiceState;

            _wordAdvanceService.CanUndo
                .SubscribeAndRegister(this, static (canUndo, self) => self._previousCardButton.SetActive(canUndo));

            _progressRepository.LearnedWordCounts[practiceState].SubscribeAndRegister(this,
                static (wordsCount, self) => self.UpdateLearnedText(wordsCount));

            _previousCardButton.OnClickAsObservable()
                .Subscribe(_wordAdvanceService.UndoCommand, static (unit, undo) => undo.Execute(unit))
                .RegisterTo(destroyCancellationToken);

            _moreButton.OnClickAsObservable().SubscribeAndRegister(this, static self => self.OpenWordControlPopUp());
        }

        private void OpenWordControlPopUp()
        {
            var wordControlPopUp = _windowsController.OpenPopUp<WordControlPopUp>();
            var currentWord = _currentWordsService.CurrentWordsByState.CurrentValue[_currentPracticeState];
            wordControlPopUp.SetParameters(currentWord);
        }

        private void UpdateLearnedText(int wordsCount)
        {
            var localization = _localizationKeysDatabase.GetLearnedCountLocalization(wordsCount);
            _learnedText.SetTextFormat(localization, wordsCount);
        }
    }
}