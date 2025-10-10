using R3;
using Source.Scripts.Core.Configs;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IModuleStateService
    {
        ReadOnlyReactiveProperty<ModuleType> CurrentState { get; }
        void SetState(ModuleType moduleType);
    }
}