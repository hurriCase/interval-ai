using System.Collections.Generic;

namespace Source.Scripts.Core.Repositories.Words.Word
{
    internal interface IWordStateMutator
    {
        void AdvanceLearningState(WordEntry word, bool success);
        void HideWord(WordEntry word);
        void ResetWord(WordEntry word);
        void SetCategories(WordEntry word, List<int> categoryIds);
    }
}