using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Base.Tests.Base;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Random = UnityEngine.Random;

namespace Source.Scripts.Core.Repositories.Base.Tests
{
    //TODO:<Dmitriy.Sukharev> create real tests
    internal sealed class TestDataFactory : ITestDataFactory
    {
        private readonly IProgressRepository _progressRepository;
        private readonly ITestDataConfig _testDataConfig;

        private EnumArray<LearningState, int> _randomProgress;

        internal TestDataFactory(IProgressRepository progressRepository, ITestDataConfig testDataConfig)
        {
            _progressRepository = progressRepository;
            _testDataConfig = testDataConfig;
        }

        public void CreateFakeProgress()
        {
            var today = DateTime.Now.Date;

            for (var i = 0; i < 30; i++)
            {
                if (Random.Range(0, 2) == 0)
                    continue;

                var date = today.AddDays(-i);

                foreach (var (learningState, wordCount) in _testDataConfig.WordsCountByState.AsTuples())
                {
                    for (var j = 0; j < wordCount.RandomValue; j++)
                        _progressRepository.IncrementDailyProgress(learningState, date);
                }
            }
        }
    }
}