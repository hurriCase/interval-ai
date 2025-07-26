using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Data.Repositories.User;
using Source.Scripts.Data.Repositories.Words;

namespace Source.Scripts.Onboarding.Source.Scripts.Onboarding.Data.Base
{
    internal interface ILocalizationDatabase
    {
        EnumArray<Language, string> Languages { get; }
        EnumArray<LanguageLevel, string> LanguageLevelKeys { get; }
    }
}