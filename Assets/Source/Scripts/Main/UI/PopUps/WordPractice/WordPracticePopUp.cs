using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Base.CurrentWord;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice
{
    internal sealed class WordPracticePopUp : PopUpBase
    {
        [SerializeField]
        private EnumArray<PracticeState, PracticeBehaviour> _practiceBehaviours = new(EnumMode.SkipFirst);

        [SerializeField] private TabsController<PracticeState> _tabsController;

        private IPracticeStateService _practiceStateService;
        private ICurrentWordFactory _currentWordFactory;

        [Inject]
        internal void Inject(IPracticeStateService practiceStateService, ICurrentWordFactory currentWordFactory)
        {
            _practiceStateService = practiceStateService;
            _currentWordFactory = currentWordFactory;
        }

        internal override void Init()
        {
            _tabsController.Init(_practiceStateService.CurrentState.CurrentValue, destroyCancellationToken);

            foreach (var practiceBehaviour in _practiceBehaviours)
                practiceBehaviour.Init();

            _practiceStateService.CurrentState.SubscribeUntilDestroy(this,
                static (state, self) => self.SwitchToState(state));
        }

        internal override async UniTask ShowAsync()
        {
            var newWordsCurrentWord = _currentWordFactory.GetOrCreate(PracticeState.NewWords);
            var reviewCurrentWord = _currentWordFactory.GetOrCreate(PracticeState.Review);

            newWordsCurrentWord.UpdateCurrentWord();
            reviewCurrentWord.UpdateCurrentWord();

            var hasNewWords = newWordsCurrentWord.HasWord();
            var hasReviewWords = reviewCurrentWord.HasWord();

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

            _tabsController.SwitchState(state, isInstant);
        }
    }
}