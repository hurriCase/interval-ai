using System;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using Source.Scripts.Core.Repositories.Categories;
using Source.Scripts.Core.Repositories.Words.Base;

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
        string GetLocalization(Type enumType, int enumIndex);
    }
}