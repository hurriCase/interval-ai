using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Core.Repositories.Words.Base.CurrentWord
{
    internal interface ICurrentWordFactory
    {
        ICurrentWordService GetOrCreate(PracticeState practiceState);
    }
}