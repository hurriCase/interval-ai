using CustomUtils.Runtime.CustomBehaviours;
using Cysharp.Threading.Tasks;
using PrimeTween;
using R3;
using R3.Triggers;
using Source.Scripts.UI.Selectables;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Practice.Behaviours
{
    internal sealed class WordCardBehaviour : RectTransformBehaviour
    {
        [SerializeField] private RectTransform _cardsContainer;

        [SerializeField] private float _spacingBetweenTabsRatio;
        [SerializeField] private float _animationDuration;

        private PracticeState _currentState = PracticeState.NewWords;
        private bool _isTransitioning;

        internal void Init(ThemeToggle learningToggle, ThemeToggle repetitionToggle)
        {
            learningToggle.OnPointerClickAsObservable()
                .Where(this,
                    (_, popUp) => popUp._isTransitioning is false && popUp._currentState != PracticeState.NewWords)
                .Subscribe(this, (_, popUp) => popUp.SwitchToState(PracticeState.NewWords).Forget())
                .RegisterTo(destroyCancellationToken);

            repetitionToggle.OnPointerClickAsObservable()
                .Where(this,
                    (_, popUp) => popUp._isTransitioning is false && popUp._currentState != PracticeState.Repetition)
                .Subscribe(this, (_, popUp) => popUp.SwitchToState(PracticeState.Repetition).Forget())
                .RegisterTo(destroyCancellationToken);
        }

        private async UniTask SwitchToState(PracticeState state)
        {
            _currentState = state;

            var containerWidth = _cardsContainer.rect.width;
            var endValue = _currentState == PracticeState.NewWords
                ? 0
                : -(containerWidth / 2 + containerWidth / _spacingBetweenTabsRatio);

            await Tween.UIAnchoredPositionX(_cardsContainer, endValue, _animationDuration);
        }
    }
}