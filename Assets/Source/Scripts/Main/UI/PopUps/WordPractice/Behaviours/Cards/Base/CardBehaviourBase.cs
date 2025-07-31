using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.DI.Repositories.Progress.Base;
using Source.Scripts.Core.DI.Repositories.Words;
using Source.Scripts.Core.DI.Repositories.Words.Base;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.LearningComplete;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Swipe;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Modules.Base;
using Source.Scripts.UI.Components;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Base
{
    internal abstract class CardBehaviourBase : MonoBehaviour
    {
        [SerializeField] protected LearningCompleteBehaviourBase learningCompleteBehaviour;
        [SerializeField] protected GameObject cardContainer;

        [SerializeField] protected ControlButtonsBehaviour controlButtonsBehaviour;

        [SerializeField] protected TextMeshProUGUI learnedText;

        [SerializeField] private EnumArray<ModuleType, PracticeModuleBase> _practiceModules = new(EnumMode.SkipFirst);

        [SerializeField] private SwipeCardBehaviour _swipeCardBehaviour;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;

        [SerializeField] private ButtonComponent _previousCardButton;

        [Inject] protected IWordsRepository wordsRepository;
        [Inject] protected IProgressRepository progressRepository;
        [Inject] protected ILocalizationKeysDatabase localizationKeysDatabase;

        [Inject] private IWordAdvanceHelper _wordAdvanceHelper;

        public WordEntry CurrentWord { get; protected set; }

        internal void Init()
        {
            controlButtonsBehaviour.Init(this);
            _swipeCardBehaviour.Init();
            _wordProgressBehaviour.Init();

            foreach (var module in _practiceModules)
                module.Init();

            _swipeCardBehaviour.OnSwipe
                .Subscribe(this, static (direction, card) => card.HandleSwipe(direction))
                .RegisterTo(destroyCancellationToken);

            _previousCardButton.OnClickAsObservable()
                .Where(this, (_, behaviour) => behaviour._wordAdvanceHelper.HasPreviousWord())
                .Subscribe(this, static (_, behaviour) => behaviour._wordAdvanceHelper.UndoWordAdvance())
                .RegisterTo(destroyCancellationToken);

            OnInit();

            UpdateWord();
        }

        internal virtual void OnInit() { }

        internal virtual void UpdateWord()
        {
            OnWordUpdate();

            UpdateView();
        }

        protected abstract void OnWordUpdate();

        private void UpdateView()
        {
            var isComplete = CurrentWord is null || CurrentWord.Cooldown > DateTime.Now;

            SwitchState(isComplete, CurrentWord is null ? CompleteType.NoWords : CompleteType.Complete,
                CurrentWord?.Cooldown.ToShortTimeString());

            if (isComplete)
                return;

            controlButtonsBehaviour.UpdateView();
            _wordProgressBehaviour.UpdateProgress(CurrentWord);

            _previousCardButton.SetActive(_wordAdvanceHelper.HasPreviousWord());

            OnUpdateView();
        }

        protected abstract void OnUpdateView();

        internal void SwitchModule(ModuleType moduleType)
        {
            foreach (var (type, module) in _practiceModules.AsTuples())
            {
                module.SetCurrentWord(CurrentWord);
                module.SetActive(type == moduleType);
            }
        }

        private void SwitchState(bool isComplete, CompleteType type = default, string completeText = null)
        {
            cardContainer.SetActive(isComplete is false);
            learningCompleteBehaviour.SetActive(isComplete);

            if (isComplete)
                learningCompleteBehaviour.SetState(type, completeText);
        }

        private void HandleSwipe(SwipeDirection direction)
        {
            switch (direction)
            {
                case SwipeDirection.Left:
                    _wordAdvanceHelper.AdvanceWord(CurrentWord, CurrentWord.LearningState == LearningState.None);
                    break;

                case SwipeDirection.Right:
                    _wordAdvanceHelper.AdvanceWord(CurrentWord, CurrentWord.LearningState != LearningState.None);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            UpdateWord();
        }
    }
}