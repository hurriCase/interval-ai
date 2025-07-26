using Source.Scripts.Data.Repositories.Settings.Base;
using Source.Scripts.Data.Repositories.Words.Data;

namespace Source.Scripts.Data.Repositories.Words
{
    internal static class WordExtensions
    {
        internal static string GetShownWord(this WordEntry wordEntry, ISettingsRepository settingsRepository) =>
            settingsRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? wordEntry.LearningWord.Name
                : wordEntry.NativeWord.Name;

        internal static string GetHiddenWord(this WordEntry wordEntry, ISettingsRepository settingsRepository) =>
            settingsRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? wordEntry.NativeWord.Name
                : wordEntry.LearningWord.Name;

        internal static string GetShownExample(this Example example, ISettingsRepository settingsRepository) =>
            settingsRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? example.LearningExample
                : example.NativeExample;

        internal static string GetHiddenExample(this Example example, ISettingsRepository settingsRepository) =>
            settingsRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? example.NativeExample
                : example.LearningExample;
    }
}