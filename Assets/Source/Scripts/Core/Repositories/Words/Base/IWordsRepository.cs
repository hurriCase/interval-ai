using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordsRepository
    {
        public ReactiveProperty<EnumArray<LearningState, SortedSet<WordEntry>>> SortedWordsByState { get; }
        ReactiveProperty<EnumArray<PracticeState, WordEntry>> CurrentWordsByState { get; }
        List<WordEntry> GetRandomWords(WordEntry wordToSkip, int count);
    }
}