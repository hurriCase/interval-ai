using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Practice
{
    internal interface IPracticeStateService
    {
        ReadOnlyReactiveProperty<PracticeState> CurrentState { get; }
        void SetState(PracticeState state);
    }
}