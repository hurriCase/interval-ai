using R3;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordAdvanceService
    {
        ReadOnlyReactiveProperty<bool> CanUndo { get; }
        ReactiveCommand UndoCommand { get; }
        void AdvanceWord(WordEntry word, bool success);
    }
}