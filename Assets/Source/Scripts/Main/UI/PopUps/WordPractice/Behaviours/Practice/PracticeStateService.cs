using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice
{
    internal sealed class PracticeStateService : IPracticeStateService
    {
        private readonly ReactiveProperty<PracticeState> _currentState = new(PracticeState.None);

        public ReadOnlyReactiveProperty<PracticeState> CurrentState => _currentState;

        public void SetState(PracticeState state) => _currentState.Value = state;
    }
}