using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Words.Advance
{
    internal interface IWordAdvanceFactory
    {
        IWordAdvanceService GetOrCreate(PracticeState key);
    }
}