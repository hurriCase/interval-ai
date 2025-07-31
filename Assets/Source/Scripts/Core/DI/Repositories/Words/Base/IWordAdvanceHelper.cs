namespace Source.Scripts.Core.DI.Repositories.Words.Base
{
    internal interface IWordAdvanceHelper
    {
        void AdvanceWord(WordEntry word, bool success);
        void UndoWordAdvance();
        bool HasPreviousWord();
    }
}