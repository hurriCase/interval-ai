using System;
using System.Collections.Generic;
using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;

namespace Source.Scripts.Core.Repositories.Settings.Repositories
{
    internal sealed class PracticeSettingsRepository : IRepository, IDisposable, IPracticeSettingsRepository
    {
        public PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; } = new();
        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; } = new();
        public PersistentReactiveProperty<int> DailyGoal { get; } = new();
        public PersistentReactiveProperty<WordReviewSourceType> WordReviewSourceType { get; } = new();

        private readonly IDefaultSettingsConfig _defaultSettingsConfig;

        internal PracticeSettingsRepository(IDefaultSettingsConfig defaultSettingsConfig)
        {
            _defaultSettingsConfig = defaultSettingsConfig;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            var initTasks = new[]
            {
                DailyGoal.InitAsync(PersistentKeys.DailyGoalKey, token, _defaultSettingsConfig.DailyGoal),

                RepetitionByCooldown.InitAsync(
                    PersistentKeys.RepetitionByCooldownKey,
                    token,
                    _defaultSettingsConfig.Cooldowns),

                LearningDirection.InitAsync(
                    PersistentKeys.LearningDirectionKey,
                    token,
                    LearningDirectionType.LearningToNative),

                WordReviewSourceType.InitAsync(
                    PersistentKeys.WordReviewSourceTypeKey,
                    token,
                    _defaultSettingsConfig.WordReviewSourceType),
            };

            await UniTask.WhenAll(initTasks);
        }

        public void Dispose()
        {
            RepetitionByCooldown.Dispose();
            LearningDirection.Dispose();
            DailyGoal.Dispose();
            WordReviewSourceType.Dispose();
        }
    }
}