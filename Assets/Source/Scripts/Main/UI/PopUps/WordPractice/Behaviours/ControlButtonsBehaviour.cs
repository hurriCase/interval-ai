using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.UI.Components.Button;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class ControlButtonsBehaviour : MonoBehaviour
    {
        [SerializeField] private ButtonComponent _alreadyKnowButton;
        [SerializeField] private ButtonComponent _hideButton;
        [SerializeField] private ButtonComponent _learnButton;

        [SerializeField] private ButtonComponent _memorizedButton;
        [SerializeField] private ButtonComponent _forgotButton;

        [SerializeField] private GameObject _firstShowContainer;
        [SerializeField] private GameObject _otherShowContainer;

        private WordEntry CurrentWord => _currentWordsService.CurrentWordsByState.CurrentValue[_currentPracticeState];

        private PracticeState _currentPracticeState;

        private IPracticeStateService _practiceStateService;
        private ICurrentWordsService _currentWordsService;
        private IWordAdvanceService _wordAdvanceService;
        private IWordStateMutator _wordStateMutator;

        [Inject]
        internal void Inject(
            IPracticeStateService practiceStateService,
            ICurrentWordsService currentWordsService,
            IWordAdvanceService wordAdvanceService,
            IWordStateMutator wordStateMutator)
        {
            _practiceStateService = practiceStateService;
            _wordAdvanceService = wordAdvanceService;
            _wordStateMutator = wordStateMutator;
            _currentWordsService = currentWordsService;
        }

        internal void Init(PracticeState practiceState)
        {
            _currentPracticeState = practiceState;

            _hideButton.OnClickAsObservable().SubscribeAndRegister(this,
                static self => self._wordStateMutator.HideWord(self.CurrentWord));

            _currentWordsService.CurrentWordsByState
                .Select(this, (currentWordsByState, self) => currentWordsByState[self._currentPracticeState])
                .Where(currentWord => currentWord != null)
                .SubscribeAndRegister(this, static self => self.UpdateView());

            _practiceStateService.CurrentState
                .SubscribeAndRegister(this, static self => self.UpdateView());

            SubscribeAdvanceButton(_alreadyKnowButton, false);
            SubscribeAdvanceButton(_learnButton, true);
            SubscribeAdvanceButton(_memorizedButton, true);
            SubscribeAdvanceButton(_forgotButton, false);
        }

        private void UpdateView()
        {
            var isFirstShow = _currentWordsService.IsFirstShow(_currentPracticeState);

            _firstShowContainer.SetActive(isFirstShow);
            _otherShowContainer.SetActive(isFirstShow is false);
        }

        private void SubscribeAdvanceButton(Button button, bool success)
        {
            button.OnClickAsObservable().SubscribeAndRegister(this, success,
                static (success, self) => self._wordAdvanceService.AdvanceWord(self.CurrentWord, success));
        }
    }
}