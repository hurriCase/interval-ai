using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Progress.Base
{
    internal interface IProgressRepository
    {
        PersistentReactiveProperty<EnumArray<LearningState, int>> TotalCountByState { get; }
        PersistentReactiveProperty<int> DailyWordsGoal { get; }
        PersistentReactiveProperty<int> CurrentStreak { get; }
        PersistentReactiveProperty<int> BestStreak { get; }
        PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>> ProgressHistory { get; }
        int NewWordsCount { get; }
        int ReviewCount { get; }
        void AddProgressToEntry(LearningState learningState, DateTime date);
        void IncreaseTotalCount(LearningState state);
        void IncrementNewWordsCount();
        void IncrementReviewCount();
    }
}