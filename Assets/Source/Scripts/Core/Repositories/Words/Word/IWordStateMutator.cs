namespace Source.Scripts.Core.Repositories.Words.Word
{
    internal interface IWordStateMutator
    {
        void AdvanceLearningState(WordEntry word, bool success);
        void HideWord(WordEntry word);
        void ResetWord(WordEntry word);
    }
}