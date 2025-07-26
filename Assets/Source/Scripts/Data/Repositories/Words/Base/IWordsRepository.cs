using R3;
using Source.Scripts.Data.Repositories.Categories.CooldownSystem;
using ZLinq;
using ZLinq.Linq;

namespace Source.Scripts.Data.Repositories.Words.Base
{
    internal interface IWordsRepository
    {
        Observable<CooldownByLearningState> OnAvailabilityTimeUpdate { get; }
        WordEntry GetAvailableWord(LearningState learningState);

        ValueEnumerable<OrderBySkipTake<ListWhere<WordEntry>, WordEntry, float>, WordEntry>
            GetRandomWords(WordEntry wordToSkip, int count);

        void AdvanceWord(WordEntry word, bool success);
    }
}