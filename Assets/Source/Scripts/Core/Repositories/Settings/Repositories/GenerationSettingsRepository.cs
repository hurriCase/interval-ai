using System;
using System.Threading;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;

namespace Source.Scripts.Core.Repositories.Settings.Repositories
{
    internal sealed class GenerationSettingsRepository : IRepository, IDisposable, IGenerationSettingsRepository
    {
        public PersistentReactiveProperty<float> NewWordsPercentage { get; } = new();
        public PersistentReactiveProperty<LanguageType> TranslateFromLanguageType { get; } = new();
        public PersistentReactiveProperty<bool> IsHighlightNewWords { get; } = new();

        private readonly IDefaultSettingsConfig _defaultSettingsConfig;

        internal GenerationSettingsRepository(IDefaultSettingsConfig defaultSettingsConfig)
        {
            _defaultSettingsConfig = defaultSettingsConfig;
        }

        public async UniTask InitAsync(CancellationToken token)
        {
            var initTasks = new[]
            {
                NewWordsPercentage.InitAsync(
                    PersistentKeys.NewWordsPercentageKey,
                    token,
                    _defaultSettingsConfig.NewWordsPercentage),

                TranslateFromLanguageType.InitAsync(
                    PersistentKeys.TranslateFromLanguageTypeKey,
                    token,
                    _defaultSettingsConfig.TranslateFromLanguageType),

                IsHighlightNewWords.InitAsync(
                    PersistentKeys.IsHighlightNewWordsKey,
                    token,
                    _defaultSettingsConfig.IsHighlightNewWords),
            };

            await UniTask.WhenAll(initTasks);
        }

        public void Dispose()
        {
            NewWordsPercentage.Dispose();
            TranslateFromLanguageType.Dispose();
            IsHighlightNewWords.Dispose();
        }
    }
}