using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Vocabulary
{
    internal static class VocabularyExtensions
    {
        internal static string GetShownWord(this WordEntry wordEntry, IUserRepository userRepository) =>
            userRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? wordEntry.LearningWord.Name
                : wordEntry.NativeWord.Name;

        internal static string GetHiddenWord(this WordEntry wordEntry, IUserRepository userRepository) =>
            userRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? wordEntry.NativeWord.Name
                : wordEntry.LearningWord.Name;

        internal static string GetShownExample(this Example example, IUserRepository userRepository) =>
            userRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? example.LearningExample
                : example.NativeExample;

        internal static string GetHiddenExample(this Example example, IUserRepository userRepository) =>
            userRepository.LearningDirection.Value == LearningDirectionType.LearningToNative
                ? example.NativeExample
                : example.LearningExample;
    }
}