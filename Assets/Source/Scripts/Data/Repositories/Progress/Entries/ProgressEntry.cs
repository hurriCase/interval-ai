using System;
using System.Collections.Generic;
using MemoryPack;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Progress.Entries
{
    [MemoryPackable]
    internal partial struct ProgressEntry
    {
        public int DailyWordsGoal { get; set; }
        public int[] StateCounts { get; }
        public int CurrentStreak { get; set; }
        public int BestStreak { get; set; }
        public Dictionary<DateTime, DailyProgress> ProgressHistory { get; }

        public ProgressEntry(int dailyWordsGoal, int[] stateCounts, int currentStreak, int bestStreak,
            Dictionary<DateTime, DailyProgress> progressHistory)
        {
            DailyWordsGoal = dailyWordsGoal;
            StateCounts = stateCounts ?? new int[Enum.GetValues(typeof(LearningState)).Length];
            CurrentStreak = currentStreak;
            BestStreak = bestStreak;
            ProgressHistory = progressHistory;
        }
    }
}