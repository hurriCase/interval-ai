using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal interface ICompleteStateService
    {
        ReadOnlyReactiveProperty<EnumArray<PracticeState, CompleteType>> CompleteStates { get; }
    }
}