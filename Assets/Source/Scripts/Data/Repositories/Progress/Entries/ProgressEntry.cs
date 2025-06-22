using System;
using System.Collections.Generic;
using MemoryPack;

namespace Source.Scripts.Data.Repositories.Progress.Entries
{
    [MemoryPackable]
    internal partial struct ProgressEntry
    {
        public int DailyWordsGoal { get; set; }
        public Dictionary<DateTime, DailyProgress> ProgressHistory { get; }

        public ProgressEntry(int dailyWordsGoal, Dictionary<DateTime, DailyProgress> progressHistory)
        {
            DailyWordsGoal = dailyWordsGoal;
            ProgressHistory = progressHistory;
        }
    }
}