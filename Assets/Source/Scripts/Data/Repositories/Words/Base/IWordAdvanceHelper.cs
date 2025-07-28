using Source.Scripts.Data.Repositories.Words.Data;

namespace Source.Scripts.Data.Repositories.Words.Base
{
    internal interface IWordAdvanceHelper
    {
        void AdvanceWord(WordEntry word, bool success);
        void UndoWordAdvance();
        bool HasPreviousWord();
    }
}