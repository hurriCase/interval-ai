using PrimeTween;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice
{
    internal sealed class WordPracticePopUp : PopUpBase
    {
        [SerializeField] private TabComponent _newWordsTab;
        [SerializeField] private TabComponent _repetitionTab;

        [SerializeField] private PracticeBehaviour _newWordsCard;
        [SerializeField] private PracticeBehaviour _reviewCard;

        [SerializeField] private RectTransform _cardsContainer;

        [SerializeField] private float _spacingBetweenTabsRatio;
        [SerializeField] private float _switchAnimationDuration;

        [Inject] private IWordsRepository _wordsRepository;

        internal static ReactiveProperty<PracticeState> CurrentState { get; } = new(PracticeState.None);

        internal override void Init()
        {
            _newWordsCard.Init();
            _reviewCard.Init();

            _newWordsTab.OnPointerClickAsObservable()
                .Where(static _ => CurrentState.Value != PracticeState.NewWords)
                .Subscribe(this, static (_, popUp) => popUp.SwitchToState(PracticeState.NewWords))
                .RegisterTo(destroyCancellationToken);

            _repetitionTab.OnPointerClickAsObservable()
                .Where(static _ => CurrentState.Value != PracticeState.Review)
                .Subscribe(this, static (_, popUp) => popUp.SwitchToState(PracticeState.Review))
                .RegisterTo(destroyCancellationToken);

            CurrentState
                .Subscribe(this, static (state, popUp) => popUp.SwitchToState(state))
                .RegisterTo(destroyCancellationToken);
        }

        internal override void Show()
        {
            var currentWords = _wordsRepository.CurrentWordsByState.Value;
            if (currentWords[PracticeState.NewWords] is null && currentWords[PracticeState.Review] != null)
                CurrentState.Value = PracticeState.Review;
            else
                CurrentState.Value = PracticeState.NewWords;

            base.Show();
        }

        internal override void Hide()
        {
            base.Hide();

            CurrentState.Value = PracticeState.None;
        }

        private void SwitchToState(PracticeState state, bool isInstant = false)
        {
            CurrentState.Value = state;

            var containerWidth = _cardsContainer.rect.width;
            var endValue = CurrentState.Value == PracticeState.NewWords
                ? 0
                : -(containerWidth / 2 + containerWidth / _spacingBetweenTabsRatio);

            _repetitionTab.isOn = CurrentState.Value == PracticeState.Review;
            _newWordsTab.isOn = CurrentState.Value == PracticeState.NewWords;

            Tween.UIAnchoredPositionX(_cardsContainer, endValue, isInstant ? 0 : _switchAnimationDuration);
        }
    }
}