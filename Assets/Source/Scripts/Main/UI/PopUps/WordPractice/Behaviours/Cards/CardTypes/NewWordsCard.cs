using System;
using CustomUtils.Runtime.Extensions;
using R3;
using Source.Scripts.Core.DI.Repositories.Words.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.Base;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards.CardTypes
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
                    card.learningCompleteBehaviour.SetState(CompleteType.NoWords,
                        progressRepository.NewWordsCount.ToString());
                    card.cardContainer.SetActive(false);
                    card.learningCompleteBehaviour.SetActive(true);
                })
                .RegisterTo(destroyCancellationToken);
        }

        protected override void OnWordUpdate()
        {
            CurrentWord = wordsRepository.GetAvailableWord(LearningState.None)
                          ?? wordsRepository.GetAvailableWord(LearningState.CurrentlyLearning);
        }

        protected override void OnUpdateView()
        {
            SwitchModule(ModuleType.FirstShow);

            var wordsCount = progressRepository.NewWordsCount;
            var localizationKey = localizationKeysDatabase.GetLearnedCountLocalization(wordsCount);

            learnedText.text = string.Format(localizationKey, wordsCount);
        }
    }
}