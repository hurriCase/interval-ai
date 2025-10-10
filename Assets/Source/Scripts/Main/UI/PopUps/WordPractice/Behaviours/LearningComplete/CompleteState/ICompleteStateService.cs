using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete.CompleteState
{
    internal interface ICompleteStateService
    {
        ReadOnlyReactiveProperty<CompleteType> CompleteStates { get; }
    }
}