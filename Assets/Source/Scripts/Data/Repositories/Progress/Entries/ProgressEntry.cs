using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using MemoryPack;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Progress.Entries
{
    [MemoryPackable]
    internal partial struct ProgressEntry
    {
        public int DailyWordsGoal { get; set; }
        public EnumArray<LearningState, int> TotalCountByState { get; }
        public int CurrentStreak { get; set; }
        public int BestStreak { get; set; }
        public Dictionary<DateTime, DailyProgress> ProgressHistory { get; }

        public ProgressEntry(int dailyWordsGoal, EnumArray<LearningState, int> totalCountByState, int currentStreak,
            int bestStreak,
            Dictionary<DateTime, DailyProgress> progressHistory)
        {
            DailyWordsGoal = dailyWordsGoal;
            TotalCountByState = totalCountByState;
            CurrentStreak = currentStreak;
            BestStreak = bestStreak;
            ProgressHistory = progressHistory;
        }
    }
}