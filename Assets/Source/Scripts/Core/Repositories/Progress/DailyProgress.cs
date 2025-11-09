using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using MemoryPack;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Progress
{
    [MemoryPackable]
    internal partial struct DailyProgress : IEquatable<DailyProgress>
    {
        public DateOnly Date { get; }
        public bool GoalAchieved { get; set; }
        public EnumArray<LearningState, int> ProgressByState { get; private set; }

        [MemoryPackConstructor]
        public DailyProgress(EnumArray<LearningState, int> progressByState, bool goalAchieved, DateOnly date)
        {
            ProgressByState = progressByState;
            GoalAchieved = goalAchieved;
            Date = date;
        }

        public DailyProgress(DateOnly date)
        {
            Date = date;
            GoalAchieved = false;
            ProgressByState = new EnumArray<LearningState, int>();
        }

        internal void AddProgress(LearningState state)
        {
            var progressByState = ProgressByState;
            progressByState[state]++;
            ProgressByState = progressByState;
        }

        internal readonly int GetProgressCountData(LearningState state) => ProgressByState[state];

        public readonly bool Equals(DailyProgress other) =>
            Date.Equals(other.Date)
            && GoalAchieved == other.GoalAchieved
            && ProgressByState.Equals(other.ProgressByState);

        public readonly override bool Equals(object obj) => obj is DailyProgress other && Equals(other);

        public readonly override int GetHashCode() => HashCode.Combine(Date, GoalAchieved, ProgressByState);

        public static bool operator ==(DailyProgress left, DailyProgress right) => left.Equals(right);

        public static bool operator !=(DailyProgress left, DailyProgress right) => left.Equals(right) is false;
    }
}