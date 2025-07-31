using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Progress
{
    [MemoryPackable]
    internal partial struct DailyProgress
    {
        public DateTime DateTime { get; }
        public bool GoalAchieved { get; set; }
        public EnumArray<LearningState, int> ProgressByState { get; private set; }
        public int NewWordsCount { get; set; }
        public int ReviewCount { get; set; }

        [MemoryPackConstructor]
        public DailyProgress(EnumArray<LearningState, int> progressByState, bool goalAchieved, DateTime dateTime,
            int newWordsCount, int reviewCount)
        {
            ProgressByState = progressByState;
            GoalAchieved = goalAchieved;
            DateTime = dateTime;
            NewWordsCount = newWordsCount;
            ReviewCount = reviewCount;
        }

        public DailyProgress(DateTime dateTime)
        {
            DateTime = dateTime;
            GoalAchieved = false;
            ProgressByState = new EnumArray<LearningState, int>();
            NewWordsCount = 0;
            ReviewCount = 0;
        }

        internal void AddProgress(LearningState state)
        {
            var progressByState = ProgressByState;
            progressByState[state]++;
            ProgressByState = progressByState;
        }

        internal readonly int GetProgressCountData(LearningState state) => ProgressByState[state];
    }
}