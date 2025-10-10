using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.LearningComplete
{
    internal interface ICompleteServiceFactory
    {
        ICompleteStateService GetOrCreate(PracticeState practiceState);
    }
}