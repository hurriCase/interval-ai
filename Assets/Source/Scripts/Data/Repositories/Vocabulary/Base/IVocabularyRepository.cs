using System.Collections.Generic;
using R3;
using Source.Scripts.Data.Repositories.Vocabulary.CooldownSystem;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using ZLinq;
using ZLinq.Linq;

namespace Source.Scripts.Data.Repositories.Vocabulary
{
    internal interface IVocabularyRepository
    {
        Observable<CooldownByLearningState> OnAvailabilityTimeUpdate { get; }
        List<CategoryEntry> GetCategories();
        WordEntry GetAvailableWord(LearningState learningState);

        ValueEnumerable<OrderBySkipTake<ListWhere<WordEntry>, WordEntry, float>, WordEntry>
            GetRandomWords(WordEntry wordToSkip, int count);

        void AdvanceWord(WordEntry word, bool success);
        void HandleSuccess(WordEntry word);
        void HandleFailure(WordEntry word);
        void AdvanceCooldown(WordEntry word);
        void UpdateTimerForState(LearningState learningState);
    }
}