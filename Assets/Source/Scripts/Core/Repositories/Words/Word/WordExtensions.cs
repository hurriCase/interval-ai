using Source.Scripts.Core.Repositories.Settings.Base;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    internal static class WordExtensions
    {
        internal static string GetShownWord(this WordEntry wordEntry, IPracticeSettingsRepository practiceRepository) =>
            practiceRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? wordEntry.LearningWord
                : wordEntry.NativeWord;

        internal static string GetHiddenWord(this WordEntry wordEntry, IPracticeSettingsRepository practiceRepository) =>
            practiceRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? wordEntry.NativeWord
                : wordEntry.LearningWord;

        internal static string GetShownExample(
            this WordEntry wordEntry, IPracticeSettingsRepository practiceSettingsRepository) =>
            practiceSettingsRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? wordEntry.LearningExample
                : wordEntry.NativeExample;

        internal static string GetHiddenExample(this WordEntry wordEntry, IPracticeSettingsRepository practiceRepository) =>
            practiceRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? wordEntry.NativeExample
                : wordEntry.LearningExample;
    }
}