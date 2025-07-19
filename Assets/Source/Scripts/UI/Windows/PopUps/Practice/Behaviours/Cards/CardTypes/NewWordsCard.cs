using System;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Cards.Base;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Practice.Behaviours.Cards.CardTypes
{
    internal sealed class NewWordsCard : CardBehaviourBase
    {
        internal override void OnInit()
        {
            base.OnInit();

            ProgressRepository.Instance.ProgressHistory
                .AsObservable()
                .Select(progressHistory => progressHistory
                    .TryGetValue(DateTime.Now, out var todayProgress) && todayProgress.GoalAchieved)
                .DistinctUntilChanged()
                .Where(goalAchieved => goalAchieved)
                .Subscribe(this, (_, card) =>
                {
                    card.cardContainer.SetActive(false);
                    card.learningCompleteBehaviour.SetActive(true);
                })
                .RegisterTo(destroyCancellationToken);
        }

        internal override void UpdateWord()
        {
            CurrentWord = VocabularyRepository.Instance.GetAvailableWord(LearningState.None);

            if (CurrentWord is null || CurrentWord.IsValid is false)
                CurrentWord = VocabularyRepository.Instance.GetAvailableWord(LearningState.CurrentlyLearning);

            if (CurrentWord is null)
            {
                Debug.LogError("[CardBehaviour::Init] there no this window yet");
                return;
            }

            controlButtonsBehaviour.UpdateView();

            SwitchModule(ModuleType.OnlyQuestion);
        }
    }
}