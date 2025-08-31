using CustomUtils.Runtime.Extensions;
using Cysharp.Threading.Tasks;
using PrimeTween;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice
{
    internal sealed class WordPracticePopUp : PopUpBase
    {
        [SerializeField] private TabComponent _newWordsTab;
        [SerializeField] private TabComponent _reviewTab;

        [SerializeField] private PracticeBehaviour _newWordsPracticeBehaviour;
        [SerializeField] private PracticeBehaviour _reviewPracticeBehaviour;

        [SerializeField] private RectTransform _cardsContainer;

        [SerializeField] private float _spacingBetweenTabsRatio;
        [SerializeField] private float _switchAnimationDuration;

        private IPracticeStateService _practiceStateService;
        private ICurrentWordsService _currentWordsService;

        [Inject]
        internal void Inject(IPracticeStateService practiceStateService, ICurrentWordsService currentWordsService)
        {
            _practiceStateService = practiceStateService;
            _currentWordsService = currentWordsService;
        }

        internal override void Init()
        {
            _newWordsPracticeBehaviour.Init();
            _reviewPracticeBehaviour.Init();

            _newWordsTab.OnPointerClickAsObservable()
                .Where(_practiceStateService.CurrentState,
                    static (_, state) => state.CurrentValue != PracticeState.NewWords)
                .SubscribeAndRegister(this, static self => self.SwitchToState(PracticeState.NewWords));

            _reviewTab.OnPointerClickAsObservable()
                .Where(_practiceStateService.CurrentState, static (_, state)
                    => state.CurrentValue != PracticeState.Review)
                .SubscribeAndRegister(this, static self => self.SwitchToState(PracticeState.Review));

            _practiceStateService.CurrentState.SubscribeAndRegister(this,
                static (state, self) => self.SwitchToState(state));
        }

        internal override async UniTask ShowAsync()
        {
            _currentWordsService.UpdateCurrentWords();

            var hasNewWords = _currentWordsService.HasWordByState(PracticeState.NewWords);
            var hasReviewWords = _currentWordsService.HasWordByState(PracticeState.Review);

            if (hasNewWords is false && hasReviewWords)
            {
                _practiceStateService.SetState(PracticeState.Review);
                return;
            }

            _practiceStateService.SetState(PracticeState.NewWords);

            await base.ShowAsync();
        }

        private void SwitchToState(PracticeState state, bool isInstant = false)
        {
            _practiceStateService.SetState(state);

            var currentState = _practiceStateService.CurrentState.CurrentValue;
            var containerWidth = _cardsContainer.rect.width;
            var endValue = currentState == PracticeState.NewWords
                ? 0
                : -(containerWidth / 2 + containerWidth / _spacingBetweenTabsRatio);

            _reviewTab.isOn = currentState == PracticeState.Review;
            _newWordsTab.isOn = currentState == PracticeState.NewWords;

            Tween.UIAnchoredPositionX(_cardsContainer, endValue, isInstant ? 0 : _switchAnimationDuration);
        }
    }
}