using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.UI.Components;
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

        [Inject] private IWordAdvanceService _wordAdvanceService;
        [Inject] private IWordsRepository _wordsRepository;

        private WordEntry CurrentWord => _wordsRepository.CurrentWordsByState.Value[_currentPracticeState];

        private PracticeState _currentPracticeState;

        internal void Init(PracticeState practiceState)
        {
            _currentPracticeState = practiceState;

            _hideButton.OnClickAsObservable()
                .Subscribe(this, (_, self) => self.CurrentWord.IsHidden = true)
                .RegisterTo(destroyCancellationToken);

            _wordsRepository.CurrentWordsByState
                .Select(this, (currentWordsByState, self)
                    => currentWordsByState[self._currentPracticeState])
                .Where(currentWord => currentWord != null)
                .Subscribe(this, static (_, self) => self.UpdateView())
                .RegisterTo(destroyCancellationToken);

            SubscribeAdvanceButton(_alreadyKnowButton, false);
            SubscribeAdvanceButton(_learnButton, true);
            SubscribeAdvanceButton(_memorizedButton, true);
            SubscribeAdvanceButton(_forgotButton, false);
        }

        private void UpdateView()
        {
            var isFirstShow = CurrentWord.LearningState == LearningState.Default;

            _firstShowContainer.SetActive(isFirstShow);
            _otherShowContainer.SetActive(isFirstShow is false);
        }

        private void SubscribeAdvanceButton(Button button, bool success) =>
            button.OnClickAsObservable()
                .Subscribe((behaviour: this, success), (_, tuple) => tuple.behaviour.AdvanceWord(tuple.success))
                .RegisterTo(destroyCancellationToken);

        private void AdvanceWord(bool success)
        {
            _wordAdvanceService.AdvanceWord(CurrentWord, success);
        }
    }
}