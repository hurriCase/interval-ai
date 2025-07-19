using PrimeTween;
using R3;
using R3.Triggers;
using Source.Scripts.UI.Selectables;
using Source.Scripts.UI.Windows.Base;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Cards.Base;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Cards.CardTypes;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Practice
{
    internal sealed class WordPracticePopUp : PopUpBase
    {
        [SerializeField] private ThemeToggle _newWordsToggle;
        [SerializeField] private ThemeToggle _repetitionToggle;

        [SerializeField] private NewWordsCard _newWordsCard;
        [SerializeField] private ReviewCard _reviewCard;

        [SerializeField] private RectTransform _cardsContainer;

        [SerializeField] private float _spacingBetweenTabsRatio;
        [SerializeField] private float _switchAnimationDuration;

        internal static readonly Subject<PracticeState> StateChangeRequested = new();
        internal static readonly Subject<ModuleType> ModuleChangeRequested = new();

        private PracticeState _currentState = PracticeState.NewWords;

        internal override void Init()
        {
            _newWordsCard.Init();
            _reviewCard.Init();

            ModuleChangeRequested
                .Subscribe(this, (moduleType, popUp) =>
                {
                    if (popUp._currentState == PracticeState.NewWords)
                    {
                        popUp._newWordsCard.SwitchModule(moduleType);
                        return;
                    }

                    popUp._reviewCard.SwitchModule(moduleType);
                })
                .RegisterTo(destroyCancellationToken);

            _newWordsToggle.OnPointerClickAsObservable()
                .Where(this, static (_, popUp) => popUp._currentState != PracticeState.NewWords)
                .Subscribe(this, static (_, popUp) => popUp.SwitchToState(PracticeState.NewWords))
                .RegisterTo(destroyCancellationToken);

            _repetitionToggle.OnPointerClickAsObservable()
                .Where(this, static (_, popUp) => popUp._currentState != PracticeState.Review)
                .Subscribe(this, static (_, popUp) => popUp.SwitchToState(PracticeState.Review))
                .RegisterTo(destroyCancellationToken);

            StateChangeRequested
                .Subscribe(this, static (state, popUp) => popUp.SwitchToState(state))
                .RegisterTo(destroyCancellationToken);
        }

        private void SwitchToState(PracticeState state, bool isInstant = false)
        {
            _currentState = state;

            var containerWidth = _cardsContainer.rect.width;
            var endValue = _currentState == PracticeState.NewWords
                ? 0
                : -(containerWidth / 2 + containerWidth / _spacingBetweenTabsRatio);

            Tween.UIAnchoredPositionX(_cardsContainer, endValue, isInstant ? 0 : _switchAnimationDuration);
        }
    }
}