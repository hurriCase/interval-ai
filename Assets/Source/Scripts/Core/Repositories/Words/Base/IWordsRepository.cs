using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Storage;
using R3;
using Source.Scripts.Core.Repositories.Words.CooldownSystem;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordsRepository
    {
        public PersistentReactiveProperty<List<WordEntry>> WordEntries { get; }
        public EnumArray<LearningState, SortedSet<WordEntry>> SortedWordsByState { get; }
        Observable<CooldownByLearningState> OnAvailabilityTimeUpdate { get; }
        WordEntry GetAvailableWord(LearningState learningState);

        List<WordEntry> GetRandomWords(WordEntry wordToSkip, int count);
        void UpdateTimerForState(LearningState learningState);
    }
}