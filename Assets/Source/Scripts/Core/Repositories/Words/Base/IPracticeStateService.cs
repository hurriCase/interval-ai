using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IPracticeStateService
    {
        ReadOnlyReactiveProperty<PracticeState> CurrentState { get; }
        void SetState(PracticeState state);
    }
}