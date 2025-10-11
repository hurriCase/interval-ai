using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using Source.Scripts.Core.Localization.Translator.Translations;
using Source.Scripts.Core.Repositories.Words.Word;
using ZLinq;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordsRepository
    {
        ReadOnlyReactiveProperty<EnumArray<LearningState, SortedSet<WordEntry>>> SortedWordsByState { get; }
        void AddWord(TranslationSet word);
        void RemoveHiddenWord(WordEntry word);
        PooledArray<WordEntry> GetRandomWords(WordEntry wordToSkip, int count);
        void OnWordStateChanged(WordEntry word, LearningState oldState, LearningState newState);
        SaveScope CreateSaveScope();
    }
}