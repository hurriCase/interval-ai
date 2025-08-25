using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Word;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordsRepository
    {
        ReadOnlyReactiveProperty<EnumArray<PracticeState, WordEntry>> CurrentWordsByState { get; }
        void AddWord(TranslationSet word);
        void UpdateCurrentWords();
        void RemoveHiddenWord(WordEntry word);
        List<WordEntry> GetRandomWords(WordEntry wordToSkip, int count);
        void SetCurrentWord(PracticeState practiceState, WordEntry word);
        void OnWordStateChanged(WordEntry word, LearningState oldState, LearningState newState);
        bool HasWordByState(PracticeState practiceState);
    }
}