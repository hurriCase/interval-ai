using MemoryPack;

namespace Source.Scripts.Data.Entries
{
    [MemoryPackable]
    internal partial struct ProgressEntry
    {
        internal int DailyWordGoal { get; set; }
        internal int TodayLearnedWordCount { get; set; }
        internal int TotalLearnedWordCount { get; set; }
        internal int BestStrick { get; set; }
        internal int CurrentStrick { get; set; }
    }
}