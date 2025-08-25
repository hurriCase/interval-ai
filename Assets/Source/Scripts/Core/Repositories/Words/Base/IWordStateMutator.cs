using System.Collections.Generic;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordStateMutator
    {
        void AdvanceLearningState(WordEntry word, bool success);
        void HideWord(WordEntry word);
        void ResetWord(WordEntry word);
        void SetCategories(WordEntry word, List<int> categoryIds);
    }
}