using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine.Scripting;

namespace Source.Scripts.Core.Repositories.Words
{
    [Preserve]
    internal sealed class PracticeStateService : IPracticeStateService
    {
        public ReadOnlyReactiveProperty<PracticeState> CurrentState => _currentState;
        private readonly ReactiveProperty<PracticeState> _currentState = new(PracticeState.NewWords);

        public void SetState(PracticeState state) => _currentState.Value = state;
    }
}