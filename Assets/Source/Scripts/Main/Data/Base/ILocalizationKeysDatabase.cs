using Source.Scripts.Core.Localization;
using Source.Scripts.Data;
using Source.Scripts.Data.Repositories.Words;

namespace Source.Scripts.Main.Source.Scripts.Main.Data.Base
{
    internal interface ILocalizationKeysDatabase
    {
        string GetLocalization(LocalizationType type);
        string GetCompletesLocalization(PracticeState practiceState, CompleteState completeState);
        string GetLearningStateLocalization(LearningState state);
        string GetDateLocalization(DateType dateType, int count);
        string GetLearnedCountLocalization(int count);
    }
}