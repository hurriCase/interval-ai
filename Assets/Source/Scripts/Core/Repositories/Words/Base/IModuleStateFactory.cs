using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IModuleStateFactory
    {
        IModuleStateService GetOrCreate(PracticeState practiceState);
    }
}