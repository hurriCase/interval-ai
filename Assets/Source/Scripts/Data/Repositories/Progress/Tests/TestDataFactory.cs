using System;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using Random = UnityEngine.Random;

namespace Source.Scripts.Data.Repositories.Progress.Tests
{
    internal sealed class TestDataFactory : ITestDataFactory
    {
        private readonly IProgressRepository _progressRepository;

        internal TestDataFactory(IProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;

#if IS_DEBUG && UNITY_EDITOR
            CreateFakeProgress();
#endif
        }

        public void CreateFakeProgress()
        {
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

                var currentEntry = _progressRepository.ProgressHistory.Value;

                for (var j = 0; j < studiedCount; j++)
                    _progressRepository.AddProgressToEntry(LearningState.Studied, date);

                for (var j = 0; j < learningCount; j++)
                    _progressRepository.AddProgressToEntry(LearningState.CurrentlyLearning, date);

                for (var j = 0; j < repeatableCount; j++)
                    _progressRepository.AddProgressToEntry(LearningState.Repeatable, date);

                for (var j = 0; j < knownCount; j++)
                    _progressRepository.AddProgressToEntry(LearningState.AlreadyKnown, date);

                _progressRepository.ProgressHistory.Value = currentEntry;
            }
        }
    }
}