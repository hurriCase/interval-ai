using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.ModuleState
{
    internal interface IModuleStateServiceFactory
    {
        IModuleStateService GetOrCreate(PracticeState practiceState);
    }
}