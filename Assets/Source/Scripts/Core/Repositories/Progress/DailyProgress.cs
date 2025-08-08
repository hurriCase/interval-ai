using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using MemoryPack;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Progress
{
    [MemoryPackable]
    internal partial struct DailyProgress
    {
        public DateTime DateTime { get; }
        public bool GoalAchieved { get; set; }
        public EnumArray<LearningState, int> ProgressByState { get; private set; }
        public EnumArray<PracticeState, int> LearnedWordsCount { get; set; }

        [MemoryPackConstructor]
        public DailyProgress(EnumArray<LearningState, int> progressByState, bool goalAchieved, DateTime dateTime,
            EnumArray<PracticeState, int> learnedWordsCount)
        {
            ProgressByState = progressByState;
            GoalAchieved = goalAchieved;
            DateTime = dateTime;
            LearnedWordsCount = learnedWordsCount;
        }

        public DailyProgress(DateTime dateTime)
        {
            DateTime = dateTime;
            GoalAchieved = false;
            ProgressByState = new EnumArray<LearningState, int>(EnumMode.SkipFirst);
            LearnedWordsCount = new EnumArray<PracticeState, int>(EnumMode.SkipFirst);
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