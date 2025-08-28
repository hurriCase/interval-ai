using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Progress.Base
{
    internal interface IProgressRepository
    {
        ReadOnlyReactiveProperty<EnumArray<LearningState, int>> TotalCountByState { get; }
        ReadOnlyReactiveProperty<int> NewWordsDailyTarget { get; }
        ReadOnlyReactiveProperty<int> CurrentStreak { get; }
        ReadOnlyReactiveProperty<int> BestStreak { get; }
        ReadOnlyReactiveProperty<Dictionary<DateTime, DailyProgress>> ProgressHistory { get; }
        EnumArray<PracticeState, ReadOnlyReactiveProperty<int>> LearnedWordCounts { get; }
        Observable<int> GoalAchievedObservable { get; }
        ReadOnlyReactiveProperty<bool> HasDailyTarget { get; }
        void IncrementDailyProgress(LearningState learningState, DateTime date);
        void ChangeDailyTarget(int valueToAdd);
        ProgressRepository.ProgressMemento CreateMemento();
    }
}