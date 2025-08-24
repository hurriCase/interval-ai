using Cysharp.Text;
using Source.Scripts.Core.Repositories.Settings.Base;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    internal static class TranslationExtensions
    {
        internal static string GetShownText(
            this Translation translation,
            IPracticeSettingsRepository practiceSettingsRepository) =>
            practiceSettingsRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? translation.Learning
                : translation.Native;

        internal static string GetHiddenText(
            this Translation translation,
            IPracticeSettingsRepository practiceRepository) =>
            practiceRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? translation.Native
                : translation.Learning;

        internal static string GetShownText(
            this TranslationSet translation,
            IPracticeSettingsRepository practiceSettingsRepository) =>
            practiceSettingsRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? translation.Learning
                : ZString.Join(", ", translation.Natives);

        internal static string GetHiddenText(
            this TranslationSet translation,
            IPracticeSettingsRepository practiceRepository) =>
            practiceRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? ZString.Join(", ", translation.Natives)
                : translation.Learning;

        internal static string GetJoinedNativeWords(this TranslationSet translation)
            => ZString.Join(", ", translation.Natives);
    }
}