using System;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Progress
{
    internal static class ProgressDataHelper
    {
        internal static void AddProgressToEntry(ref ProgressEntry progressEntry, LearningState learningState, DateTime date)
        {
            var dateOnly = date.Date;

            if (progressEntry.ProgressHistory.TryGetValue(dateOnly, out var dailyProgress) is false)
            {
                dailyProgress = new DailyProgress(dateOnly);
                progressEntry.ProgressHistory[dateOnly] = dailyProgress;
            }

            dailyProgress.AddProgress(learningState);

            if (learningState == LearningState.CurrentlyLearning)
                ProcessNewWordProgress(ref dailyProgress, ref progressEntry);

            progressEntry.ProgressHistory[dateOnly] = dailyProgress;
            var stateCounts = progressEntry.TotalCountByState;
            stateCounts[learningState]++;
        }

        private static void ProcessNewWordProgress(ref DailyProgress dailyProgress, ref ProgressEntry progressEntry)
        {
            var progressCount = dailyProgress.ProgressByState[LearningState.CurrentlyLearning];

            if (dailyProgress.GoalAchieved || progressCount < progressEntry.DailyWordsGoal)
                return;

            progressEntry.CurrentStreak++;

            if (progressEntry.CurrentStreak > progressEntry.BestStreak)
                progressEntry.BestStreak = progressEntry.CurrentStreak;

            dailyProgress.GoalAchieved = true;
        }
    }
}