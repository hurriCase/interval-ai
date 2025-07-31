using Source.Scripts.Core.DI.Repositories.Categories;
using Source.Scripts.Core.DI.Repositories.Words.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;

namespace Source.Scripts.Core.Localization.Base
{
    internal interface ILocalizationKeysDatabase
    {
        string GetLocalization(LocalizationType type);
        string GetCompletesLocalization(PracticeState practiceState, CompleteType completeType);
        string GetLearningStateLocalization(LearningState state);
        string GetLearningStateLocalization(CategoryType categoryType);
        string GetDateLocalization(DateType dateType, int count);
        string GetLearnedCountLocalization(int count);
    }
}