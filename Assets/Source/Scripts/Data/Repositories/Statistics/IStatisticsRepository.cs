using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Storage;

namespace Source.Scripts.Data.Repositories.Statistics
{
    internal interface IStatisticsRepository
    {
        PersistentReactiveProperty<Dictionary<DateTime, bool>> LoginHistory { get; }
        PersistentReactiveProperty<bool> IsCompleteOnboarding { get; }
    }
}