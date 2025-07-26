using System;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.Localization;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Data.Repositories.Words.Base;
using Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Base;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.CardTypes
{
    internal sealed class NewWordsCard : CardBehaviourBase
    {
        internal override void OnInit()
        {
            base.OnInit();

            learningCompleteBehaviour.Init(PracticeState.NewWords);

            progressRepository.ProgressHistory
                .AsObservable()
                .Select(progressHistory => progressHistory
                    .TryGetValue(DateTime.Now, out var todayProgress) && todayProgress.GoalAchieved)
                .DistinctUntilChanged()
                .Where(goalAchieved => goalAchieved)
                .Subscribe(this, (_, card) =>
                {
                    card.learningCompleteBehaviour.SetState(CompleteState.NoWords,
                        progressRepository.NewWordsCount.ToString());
                    card.cardContainer.SetActive(false);
                    card.learningCompleteBehaviour.SetActive(true);
                })
                .RegisterTo(destroyCancellationToken);
        }

        internal override void UpdateWord()
        {
            CurrentWord = wordsRepository.GetAvailableWord(LearningState.None);

            if (CurrentWord is null || CurrentWord.IsValid is false)
                CurrentWord = wordsRepository.GetAvailableWord(LearningState.CurrentlyLearning);

            base.UpdateWord();
        }

        internal override void UpdateView()
        {
            base.UpdateView();

            SwitchModule(ModuleType.OnlyQuestion);

            var wordsCount = progressRepository.NewWordsCount;
            var localizationKey = localizationKeysDatabase.GetLearnedCountLocalization(wordsCount);

            learnedText.text = string.Format((localizationKey), wordsCount);
        }
    }
}