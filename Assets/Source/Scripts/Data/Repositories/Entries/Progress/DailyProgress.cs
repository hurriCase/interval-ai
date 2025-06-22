using System;
using MemoryPack;
using Source.Scripts.Data.Repositories.Entries.Words;

namespace Source.Scripts.Data.Repositories.Entries.Progress
{
    [MemoryPackable]
    internal readonly partial struct DailyProgress
    {
        public DateTime DateTime { get; }
        public int[] ProgressCountData { get; }

        public DailyProgress(int[] progressCountData, DateTime dateTime)
        {
            ProgressCountData = progressCountData ?? new int[Enum.GetValues(typeof(LearningState)).Length];
            DateTime = dateTime;
        }

        internal void AddProgress(LearningState state) => ProgressCountData[(int)state]++;
        internal int GetProgressCountData(LearningState state) => ProgressCountData[(int)state];
    }
}