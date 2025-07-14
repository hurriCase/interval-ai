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
        [SerializeField] private RectTransform _learningTab;
        [SerializeField] private RectTransform _repetitionTab;

        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private float _cardPadding = 20f;

        private PracticeState _currentState = PracticeState.NewWords;
        private bool _isTransitioning;

        internal void Init(ThemeToggle learningToggle, ThemeToggle repetitionToggle)
        {
            learningToggle.OnPointerClickAsObservable()
                .Where(this, (_, popUp) => popUp._isTransitioning is false && popUp._currentState != PracticeState.NewWords)
                .Subscribe(this, (_, popUp) => popUp.SwitchToState(PracticeState.NewWords).Forget())
                .RegisterTo(destroyCancellationToken);

            repetitionToggle.OnPointerClickAsObservable()
                .Where(this, (_, popUp) => popUp._isTransitioning is false && popUp._currentState != PracticeState.Repetition)
                .Subscribe(this, (_, popUp) => popUp.SwitchToState(PracticeState.Repetition).Forget())
                .RegisterTo(destroyCancellationToken);
        }

        private async UniTask SwitchToState(PracticeState newState)
        {
            if (_isTransitioning || _currentState == newState)
                return;

            _isTransitioning = true;
            _currentState = newState;

            await PerformCardTransition(newState);

            _isTransitioning = false;
        }

        private async Awaitable PerformCardTransition(PracticeState state)
        {
            var cardWidth = _learningTab.rect.width;
            var slideDistance = cardWidth + _cardPadding;

            Vector2 learningTargetPos;
            Vector2 repetitionTargetPos;

            switch (state)
            {
                case PracticeState.NewWords:
                    learningTargetPos = Vector2.zero;
                    repetitionTargetPos = new Vector2(slideDistance, 0);
                    break;

                case PracticeState.Repetition:
                    learningTargetPos = new Vector2(-slideDistance, 0);
                    repetitionTargetPos = Vector2.zero;
                    break;

                default:
                    return;
            }

            await Tween.LocalPosition(_learningTab, learningTargetPos, _animationDuration);

            await Tween.LocalPosition(_repetitionTab, repetitionTargetPos, _animationDuration);
        }
    }
}