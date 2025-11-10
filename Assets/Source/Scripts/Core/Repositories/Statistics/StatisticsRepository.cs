using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Constants;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Repositories.Base;

namespace Source.Scripts.Core.Repositories.Statistics
{
    internal sealed class StatisticsRepository : IStatisticsRepository, IRepository, IDisposable
    {
        public PersistentReactiveProperty<bool> IsCompleteOnboarding { get; } = new();
        public Observable<Unit> OnNewLogin => _newLogin;

        private readonly PersistentReactiveProperty<Dictionary<DateOnly, bool>> _loginHistory = new();

        private readonly Subject<Unit> _newLogin = new();

        public async UniTask InitAsync(CancellationToken token)
        {
            var initTasks = new[]
            {
                IsCompleteOnboarding.InitAsync(PersistentKeys.IsCompleteOnboardingKey, token),

                _loginHistory.InitAsync(
                    PersistentKeys.LoginHistoryKey,
                    token,
                    new Dictionary<DateOnly, bool>())
            };

            await UniTask.WhenAll(initTasks);
        }

        public void MarkNewLogin()
        {
            if (_loginHistory.Value.TryGetValue(Date.Today, out var isNewLogin) && isNewLogin)
                return;

            _loginHistory.Value[Date.Today] = true;
            _loginHistory.SaveAsync();

            _newLogin.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            IsCompleteOnboarding.Dispose();
            _loginHistory.Dispose();
            _newLogin.Dispose();
        }
    }
}