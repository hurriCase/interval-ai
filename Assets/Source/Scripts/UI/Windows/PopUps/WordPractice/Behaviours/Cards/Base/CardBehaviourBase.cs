using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.LearningComplete;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.Swipe;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Modules.Base;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.Base
{
    internal class CardBehaviourBase : MonoBehaviour
    {
        [SerializeField] protected LearningCompleteBehaviourBase learningCompleteBehaviour;
        [SerializeField] protected GameObject cardContainer;

        [SerializeField] private EnumArray<ModuleType, PracticeModuleBase> _practiceModules = new(EnumMode.SkipFirst);

        [SerializeField] protected ControlButtonsBehaviour controlButtonsBehaviour;
        [SerializeField] private SwipeCardBehaviour _swipeCardBehaviour;

        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;

        public WordEntry CurrentWord { get; protected set; }

        internal void Init()
        {
            controlButtonsBehaviour.Init(this);
            _wordProgressBehaviour.Init();

            learningCompleteBehaviour.Init();
            learningCompleteBehaviour.SetActive(false);

            foreach (var module in _practiceModules)
                module.Init();

            _swipeCardBehaviour.OnSwipe
                .Subscribe(this, static (direction, card) => card.HandleSwipe(direction))
                .RegisterTo(destroyCancellationToken);

            OnInit();

            UpdateWord();
        }

        internal virtual void OnInit() { }

        internal virtual void UpdateWord()
        {
            controlButtonsBehaviour.UpdateView();
            _wordProgressBehaviour.UpdateProgress(CurrentWord);
        }

        internal void SwitchModule(ModuleType moduleType)
        {
            foreach (var (type, module) in _practiceModules.AsTuples())
            {
                module.SetCurrentWord(CurrentWord);
                module.SetActive(type == moduleType);
            }
        }

        private void HandleSwipe(SwipeDirection direction)
        {
            switch (direction)
            {
                case SwipeDirection.Left:
                    controlButtonsBehaviour.AdvanceWord();
                    break;

                case SwipeDirection.Right:
                    controlButtonsBehaviour.SwitchToNext();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}