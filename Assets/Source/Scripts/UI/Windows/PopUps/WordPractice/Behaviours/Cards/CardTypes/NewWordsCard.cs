using System;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Vocabulary;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Source.Scripts.UI.Localization;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.Base;
using Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.LearningComplete;

namespace Source.Scripts.UI.Windows.PopUps.WordPractice.Behaviours.Cards.CardTypes
{
    internal sealed class NewWordsCard : CardBehaviourBase
    {
        internal override void OnInit()
        {
            base.OnInit();

            learningCompleteBehaviour.Init(PracticeState.NewWords);

            ProgressRepository.Instance.ProgressHistory
                .AsObservable()
                .Select(progressHistory => progressHistory
                    .TryGetValue(DateTime.Now, out var todayProgress) && todayProgress.GoalAchieved)
                .DistinctUntilChanged()
                .Where(goalAchieved => goalAchieved)
                .Subscribe(this, (_, card) =>
                {
                    card.learningCompleteBehaviour.SetState(CompleteState.NoWords,
                        ProgressRepository.Instance.NewWordsCount.ToString());
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

            base.UpdateWord();
        }

        internal override void UpdateView()
        {
            base.UpdateView();

            SwitchModule(ModuleType.OnlyQuestion);

            var wordsCount = ProgressRepository.Instance.NewWordsCount;
            var localizationKey = LocalizationKeysDatabase.Instance.GetLearnedCountLocalization(wordsCount);

            learnedText.text = string.Format(LocalizationController.Localize(localizationKey), wordsCount);
        }
    }
}