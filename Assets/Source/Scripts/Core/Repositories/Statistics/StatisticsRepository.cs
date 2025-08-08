using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base;

namespace Source.Scripts.Core.Repositories.Statistics
{
    internal sealed class StatisticsRepository : IStatisticsRepository, IRepository
    {
        public PersistentReactiveProperty<bool> IsCompleteOnboarding { get; } = new();
        public PersistentReactiveProperty<Dictionary<DateTime, bool>> LoginHistory { get; } = new();

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                IsCompleteOnboarding.InitAsync(PersistentKeys.IsCompleteOnboardingKey, cancellationToken),

                LoginHistory.InitAsync(
                    PersistentKeys.LoginHistoryKey,
                    cancellationToken,
                    new Dictionary<DateTime, bool>())
            };

            await UniTask.WhenAll(initTasks);
        }

        public void Dispose()
        {
            LoginHistory.Dispose();
            IsCompleteOnboarding.Dispose();
        }
    }
}