using System;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Scripts.Data
{
    internal static class TestDataFactory
    {
        public static void CreateFakeProgress()
        {
            var progressRepo = ProgressRepository.Instance;
            var today = DateTime.Now.Date;

            for (var i = 0; i < 30; i++)
            {
                if (Random.Range(0, 2) == 0)
                    continue;

                var date = today.AddDays(-i);

                var studiedCount = Random.Range(0, 15);
                var learningCount = Random.Range(0, 8);
                var repeatableCount = Random.Range(0, 5);
                var knownCount = Random.Range(0, 3);

                for (var j = 0; j < studiedCount; j++)
                    progressRepo.AddProgressForDate(LearningState.Studied, date);

                for (var j = 0; j < learningCount; j++)
                    progressRepo.AddProgressForDate(LearningState.CurrentlyLearning, date);

                for (var j = 0; j < repeatableCount; j++)
                    progressRepo.AddProgressForDate(LearningState.Repeatable, date);

                for (var j = 0; j < knownCount; j++)
                    progressRepo.AddProgressForDate(LearningState.AlreadyKnown, date);
            }
        }

        private static void AddProgressForDate(this ProgressRepository repository,
            LearningState learningState, DateTime date)
        {
            var currentEntry = repository.ProgressEntry.Value;
            var dateOnly = date.Date;

            if (currentEntry.ProgressHistory.TryGetValue(dateOnly, out var dailyProgress) is false)
            {
                dailyProgress = new DailyProgress(dateOnly);
                currentEntry.ProgressHistory[dateOnly] = dailyProgress;
            }

            dailyProgress.AddProgress(learningState);
            currentEntry.ProgressHistory[dateOnly] = dailyProgress;
            repository.ProgressEntry.Value = currentEntry;
        }
    }
}