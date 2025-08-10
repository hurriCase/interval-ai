using R3;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordAdvanceService
    {
        ReadOnlyReactiveProperty<bool> CanUndo { get; }
        ReactiveCommand UndoCommand { get; }
        void AdvanceWord(WordEntry word, bool success);
    }
}