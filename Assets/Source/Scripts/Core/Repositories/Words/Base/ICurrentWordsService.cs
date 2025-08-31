using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface ICurrentWordsService
    {
        ReadOnlyReactiveProperty<EnumArray<PracticeState, WordEntry>> CurrentWordsByState { get; }
        void UpdateCurrentWords();
        void SetCurrentWord(PracticeState practiceState, WordEntry word);
        bool HasWordByState(PracticeState practiceState);
        bool IsFirstShow(PracticeState practiceState);
    }
}