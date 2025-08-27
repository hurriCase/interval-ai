using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal static class LearningStateExtensions
    {
        internal static bool IsFirstShown(this LearningState learningState, PracticeState practiceState)
            => learningState == LearningState.Default && practiceState == PracticeState.NewWords;
    }
}