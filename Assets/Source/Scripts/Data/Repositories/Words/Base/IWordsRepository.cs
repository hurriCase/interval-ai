using R3;
using Source.Scripts.Data.Repositories.Categories.CooldownSystem;
using ZLinq;
using ZLinq.Linq;

namespace Source.Scripts.Data.Repositories.Words.Base
{
    internal interface IWordsRepository
    {
        Observable<CooldownByLearningState> OnAvailabilityTimeUpdate { get; }
        Data.WordEntry GetAvailableWord(LearningState learningState);

        ValueEnumerable<OrderBySkipTake<ListWhere<Data.WordEntry>, Data.WordEntry, float>, Data.WordEntry>
            GetRandomWords(Data.WordEntry wordToSkip, int count);

        void AdvanceWord(Data.WordEntry word, bool success);
    }
}