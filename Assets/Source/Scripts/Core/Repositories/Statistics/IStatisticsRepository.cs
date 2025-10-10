using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Storage;
using R3;

namespace Source.Scripts.Core.Repositories.Statistics
{
    internal interface IStatisticsRepository
    {
        PersistentReactiveProperty<Dictionary<DateTime, bool>> LoginHistory { get; }
        PersistentReactiveProperty<bool> IsCompleteOnboarding { get; }
        Observable<Unit> OnNewLogin { get; }
    }
}