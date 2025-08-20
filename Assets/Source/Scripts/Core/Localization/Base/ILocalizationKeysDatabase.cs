using System;
using JetBrains.Annotations;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using Source.Scripts.Core.Repositories.Categories.Base;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Localization.Base
{
    internal interface ILocalizationKeysDatabase
    {
        [MustUseReturnValue]
        string GetLocalization(LocalizationType type);

        [MustUseReturnValue]
        string GetCompletesLocalization(PracticeState practiceState, CompleteType completeType);

        [MustUseReturnValue]
        string GetLearningStateLocalization(LearningState state);

        [MustUseReturnValue]
        string GetLearningStateLocalization(CategoryType categoryType);

        [MustUseReturnValue]
        string GetDateLocalization(DateType dateType, int count);

        [MustUseReturnValue]
        string GetLearnedCountLocalization(int count);

        [MustUseReturnValue]
        string GetLocalizationByValue<TEnum>(TEnum enumValue)
            where TEnum : unmanaged, Enum;
    }
}