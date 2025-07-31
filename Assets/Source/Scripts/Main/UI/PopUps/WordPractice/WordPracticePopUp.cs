using PrimeTween;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.CardTypes;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.WordPractice
{
    internal sealed class WordPracticePopUp : PopUpBase
    {
        [SerializeField] private TabComponent _newWordsTab;
        [SerializeField] private TabComponent _repetitionTab;

        [SerializeField] private NewWordsCard _newWordsCard;
        [SerializeField] private ReviewCard _reviewCard;

        [SerializeField] private RectTransform _cardsContainer;

        [SerializeField] private float _spacingBetweenTabsRatio;
        [SerializeField] private float _switchAnimationDuration;

        internal static readonly Subject<ModuleType> ModuleTypeChangeSubject = new();

        internal static ReactiveProperty<PracticeState> CurrentState { get; } = new(PracticeState.None);

        internal override void Init()
        {
            _newWordsCard.Init();
            _reviewCard.Init();

            ModuleTypeChangeSubject
                .Subscribe(this, (moduleType, popUp) =>
                {
                    if (CurrentState.Value == PracticeState.NewWords)
                    {
                        popUp._newWordsCard.SwitchModule(moduleType);
                        return;
                    }

                    popUp._reviewCard.SwitchModule(moduleType);
                })
                .RegisterTo(destroyCancellationToken);

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
            if (_newWordsCard.CurrentWord is null && _reviewCard.CurrentWord != null)
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