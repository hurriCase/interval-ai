namespace Source.Scripts.Data
{
    internal static class PersistentPropertyKeys
    {
        internal const string TotalCountByStateKey = Prefix + "totalCountByStateStreak";
        internal const string BestStreakKey = Prefix + "bestStreak";
        internal const string CurrentStreakKey = Prefix + "currentStreak";
        internal const string DailyGoalKey = Prefix + "dailyGoal";
        internal const string ProgressEntryKey = Prefix + "progressEntry";
        internal const string WordEntryKey = Prefix + "wordEntry";
        internal const string CategoryEntryKey = Prefix + "categoryEntry";

        private const string Prefix = "LanguageLearningApp.";
    }
}