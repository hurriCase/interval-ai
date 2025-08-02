using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Storage;
using Source.Scripts.Core.Repositories.Statistics;

namespace Source.Scripts.Data.Repositories.Statistics
{
    internal sealed class StatisticsRepository : IDisposable, IStatisticsRepository
    {
        public PersistentReactiveProperty<bool> IsCompleteOnboarding { get; }
        public PersistentReactiveProperty<Dictionary<DateTime, bool>> LoginHistory { get; }

        internal StatisticsRepository()
        {
            IsCompleteOnboarding = new PersistentReactiveProperty<bool>(PersistentKeys.IsCompleteOnboardingKey);
            LoginHistory =
                new PersistentReactiveProperty<Dictionary<DateTime, bool>>(PersistentKeys.LoginHistoryKey,
                    new Dictionary<DateTime, bool>());
        }

        public void Dispose()
        {
            LoginHistory.Dispose();
            IsCompleteOnboarding.Dispose();
        }
    }
}