using R3;
using Source.Scripts.Core.Repositories.Words.CooldownSystem;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordsTimerService
    {
        Observable<CooldownByPracticeState> OnAvailabilityTimeUpdate { get; }
        void UpdateTimers();
    }
}