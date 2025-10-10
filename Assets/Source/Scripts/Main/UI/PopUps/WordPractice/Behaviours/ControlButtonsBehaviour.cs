using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Advance;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
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

        private WordEntry CurrentWord => _currentWordService.CurrentWord.CurrentValue;

        private ICurrentWordService _currentWordService;
        private IWordAdvanceFactory _wordAdvanceFactory;

        private IPracticeStateService _practiceStateService;
        private ICurrentWordFactory _currentWordFactory;
        private IWordAdvanceService _wordAdvanceService;
        private IWordStateMutator _wordStateMutator;

        [Inject]
        internal void Inject(
            IPracticeStateService practiceStateService,
            ICurrentWordFactory currentWordFactory,
            IWordAdvanceFactory wordAdvanceFactory,
            IWordStateMutator wordStateMutator)
        {
            _practiceStateService = practiceStateService;
            _currentWordFactory = currentWordFactory;
            _wordAdvanceFactory = wordAdvanceFactory;
            _wordStateMutator = wordStateMutator;
        }

        internal void Init(PracticeState practiceState)
        {
            _currentWordService = _currentWordFactory.GetOrCreate(practiceState);
            _wordAdvanceService = _wordAdvanceFactory.GetOrCreate(practiceState);

            _hideButton.OnClickAsObservable().SubscribeUntilDestroy(this,
                static self => self._wordStateMutator.HideWord(self.CurrentWord));

            _currentWordService.CurrentWord
                .Where(currentWord => currentWord != null)
                .SubscribeUntilDestroy(this, static self => self.UpdateView());

            _practiceStateService.CurrentState
                .SubscribeUntilDestroy(this, static self => self.UpdateView());

            SubscribeAdvanceButton(_alreadyKnowButton, false);
            SubscribeAdvanceButton(_learnButton, true);
            SubscribeAdvanceButton(_memorizedButton, true);
            SubscribeAdvanceButton(_forgotButton, false);
        }

        private void UpdateView()
        {
            var isFirstShow = _currentWordService.IsFirstShow();

            _firstShowContainer.SetActive(isFirstShow);
            _otherShowContainer.SetActive(isFirstShow is false);
        }

        private void SubscribeAdvanceButton(Button button, bool success)
        {
            button.OnClickAsObservable().SubscribeUntilDestroy(this, success,
                static (success, self) => self._wordAdvanceService.AdvanceWord(self.CurrentWord, success));
        }
    }
}