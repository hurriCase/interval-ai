using R3;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Words.Base.CurrentWord
{
    internal interface ICurrentWordService
    {
        ReadOnlyReactiveProperty<WordEntry> CurrentWord { get; }
        void UpdateCurrentWord();
        void SetCurrentWord(WordEntry word);
        bool HasWord();
        bool IsFirstShow();
    }
}