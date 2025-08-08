using R3;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordAdvanceService
    {
        Observable<bool> CanUndo { get; }
        void AdvanceWord(WordEntry word, bool success);
        void UndoWordAdvance();
    }
}