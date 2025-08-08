namespace Source.Scripts.Core.Repositories.Base
{
    internal static class PersistentKeys
    {
        internal const string CategoryEntriesKey = Prefix + "CategoryEntriesKey";

        internal const string CurrentStreakKey = Prefix + "CurrentStreakKey";
        internal const string BestStreakKey = Prefix + "BestStreakKey";
        internal const string TotalCountByStateKey = Prefix + "TotalCountByStateKey";
        internal const string NewWordsDailyTargetKey = Prefix + "NewWordsDailyTargetKey";
        internal const string ProgressHistoryKey = Prefix + "ProgressHistoryKey";

        internal const string LanguageLevelKey = Prefix + "LanguageLevelKey";
        internal const string DailyGoalKey = Prefix + "DailyGoalKey";
        internal const string CurrentCultureKey = Prefix + "CurrentCultureKey";
        internal const string RepetitionByCooldownKey = Prefix + "RepetitionByCooldownKey";
        internal const string LanguageByTypeKey = Prefix + "LanguageByTypeKey";
        internal const string LearningDirectionKey = Prefix + "LearningDirectionKey";

        internal const string IsCompleteOnboardingKey = Prefix + "IsCompleteOnboardingKey";
        internal const string LoginHistoryKey = Prefix + "LoginHistoryKey";

        internal const string UserIconKey = Prefix + "UserIconKey";
        internal const string NicknameKey = Prefix + "NicknameKey";

        internal const string WordEntryKey = Prefix + "WordEntryKey";

        internal const string CurrentMaxIdKey = Prefix + "CurrentMaxIdKey";

        private const string Prefix = "LanguageLearningApp.";
    }
}