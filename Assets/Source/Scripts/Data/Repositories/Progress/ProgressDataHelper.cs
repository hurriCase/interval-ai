using System;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal static class ProgressDataHelper
    {
        internal static void AddProgressToEntry(LearningState learningState, DateTime date)
        {
            var repository = ProgressRepository.Instance;
            var dateOnly = date.Date;

            if (repository.ProgressHistory.Value.TryGetValue(dateOnly, out var dailyProgress) is false)
            {
                dailyProgress = new DailyProgress(dateOnly);
                repository.ProgressHistory.Value[dateOnly] = dailyProgress;
            }

            dailyProgress.AddProgress(learningState);

            if (learningState == LearningState.CurrentlyLearning)
                ProcessNewWordProgress(ref dailyProgress, repository);

            repository.ProgressHistory.Value[dateOnly] = dailyProgress;
            repository.IncreaseTotalCount(learningState);
        }

        private static void ProcessNewWordProgress(ref DailyProgress dailyProgress, ProgressRepository repository)
        {
            var progressCount = dailyProgress.ProgressByState[LearningState.CurrentlyLearning];

            if (dailyProgress.GoalAchieved || progressCount < repository.DailyWordsGoal.Value)
                return;

            repository.CurrentStreak.Value++;

            if (repository.CurrentStreak.Value > repository.BestStreak.Value)
                repository.BestStreak.Value = repository.CurrentStreak.Value;

            dailyProgress.GoalAchieved = true;
        }
    }
}