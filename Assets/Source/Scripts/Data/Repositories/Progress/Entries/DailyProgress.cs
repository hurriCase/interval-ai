using System;
using MemoryPack;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Progress.Entries
{
    [MemoryPackable]
    internal partial struct DailyProgress
    {
        public DateTime DateTime { get; }
        public bool GoalAchieved { get; set; }
        public int[] ProgressCountData { get; }

        [MemoryPackConstructor]
        public DailyProgress(int[] progressCountData, bool goalAchieved, DateTime dateTime)
        {
            ProgressCountData = progressCountData ?? new int[Enum.GetValues(typeof(LearningState)).Length];
            GoalAchieved = goalAchieved;
            DateTime = dateTime;
        }

        public DailyProgress(DateTime dateTime)
        {
            ProgressCountData = new int[Enum.GetValues(typeof(LearningState)).Length];
            GoalAchieved = false;
            DateTime = dateTime;
        }

        internal readonly void AddProgress(LearningState state) => ProgressCountData[(int)state]++;
        internal readonly int GetProgressCountData(LearningState state) => ProgressCountData[(int)state];
    }
}