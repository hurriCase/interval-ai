using System;
using Source.Scripts.Data.Repositories.Progress;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
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

                var currentEntry = progressRepo.ProgressEntry.Value;

                for (var j = 0; j < studiedCount; j++)
                    ProgressDataHelper.AddProgressToEntry(currentEntry, LearningState.Studied, date);

                for (var j = 0; j < learningCount; j++)
                    ProgressDataHelper.AddProgressToEntry(currentEntry, LearningState.CurrentlyLearning, date);

                for (var j = 0; j < repeatableCount; j++)
                    ProgressDataHelper.AddProgressToEntry(currentEntry, LearningState.Repeatable, date);

                for (var j = 0; j < knownCount; j++)
                    ProgressDataHelper.AddProgressToEntry(currentEntry, LearningState.AlreadyKnown, date);

                progressRepo.ProgressEntry.Value = currentEntry;
            }
        }
    }
}