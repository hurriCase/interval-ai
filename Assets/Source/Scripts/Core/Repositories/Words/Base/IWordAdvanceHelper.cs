namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordAdvanceHelper
    {
        void AdvanceWord(WordEntry word, bool success);
        void UndoWordAdvance();
        bool HasPreviousWord();
    }
}