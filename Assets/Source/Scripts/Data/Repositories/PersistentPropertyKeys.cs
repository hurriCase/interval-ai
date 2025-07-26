namespace Source.Scripts.Data.Repositories
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

        internal const string LearningDirectionKey = Prefix + "learningDirection";
        internal const string RepetitionByCooldownKey = Prefix + "repetitionByCooldown";
        internal const string UserIcon = Prefix + "repetitionByCooldown";
        internal const string UserLevelKey = Prefix + "userLevel";
        internal const string IsCompleteOnboardingKey = Prefix + "isCompleteOnboarding";
        internal const string LoginHistoryKey = Prefix + "loginHistory";
        internal const string LanguageByTypeKey = Prefix + "languageByType";
        internal const string LearningLanguageKey = Prefix + "learningLanguage";
        internal const string UserNameKey = Prefix + "userName";
        internal const string CurrentCultureKey = Prefix + "currentCulture";

        private const string Prefix = "LanguageLearningApp.";
    }
}