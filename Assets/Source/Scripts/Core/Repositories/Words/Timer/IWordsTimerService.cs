using R3;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.CooldownSystem;

namespace Source.Scripts.Core.Repositories.Words.Timer
{
    internal interface IWordsTimerService
    {
        Observable<CooldownByLearningState> OnAvailabilityTimeUpdate { get; }
        void Init(IWordsRepository wordsRepository);
        void UpdateTimerForState(LearningState learningState);
    }
}