using System;
using R3;

namespace Source.Scripts.Core.Repositories.Words.Base
{
    internal interface IWordsTimerService
    {
        Observable<DateTime> OnAvailabilityTimeUpdate { get; }
        void UpdateTimer();
    }
}