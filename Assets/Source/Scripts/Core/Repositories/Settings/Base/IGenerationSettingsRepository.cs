using CustomUtils.Runtime.Storage;

namespace Source.Scripts.Core.Repositories.Settings.Base
{
    internal interface IGenerationSettingsRepository
    {
        PersistentReactiveProperty<float> NewWordsPercentage { get; }
        PersistentReactiveProperty<LanguageType> TranslateFromLanguageType { get; }
        PersistentReactiveProperty<bool> IsHighlightNewWords { get; }
    }
}