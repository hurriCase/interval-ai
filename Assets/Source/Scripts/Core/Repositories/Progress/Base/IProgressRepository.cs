using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Progress.Base
{
    internal interface IProgressRepository
    {
        PersistentReactiveProperty<EnumArray<LearningState, int>> TotalCountByState { get; }
        PersistentReactiveProperty<int> NewWordsDailyTarget { get; }
        PersistentReactiveProperty<int> CurrentStreak { get; }
        PersistentReactiveProperty<int> BestStreak { get; }
        PersistentReactiveProperty<Dictionary<DateTime, DailyProgress>> ProgressHistory { get; }
        int NewWordsCount { get; }
        int ReviewCount { get; }
        void IncrementDailyProgress(LearningState learningState, DateTime date);
        void IncrementNewWordsCount();
        void IncrementReviewCount();
    }
}