using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.Storage;
using CustomUtils.Runtime.UI.Theme.Base;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Settings
{
    internal sealed class SettingsRepository : ISettingsRepository, IRepository
    {
        public PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; } = new();
        public PersistentReactiveProperty<int> DailyGoal { get; } = new();
        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; } = new();
        public PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; } = new();
        public PersistentReactiveProperty<EnumArray<LanguageType, SystemLanguage>> LanguageByType { get; } = new();
        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; } = new();
        public PersistentReactiveProperty<ThemeType> CurrentTheme { get; } = new();

        public PersistentReactiveProperty<bool> IsSendNotifications { get; } = new();
        public PersistentReactiveProperty<bool> IsShowTranscription { get; } = new();
        public PersistentReactiveProperty<bool> IsSwipeEnabled { get; } = new();

        private readonly IDefaultSettingsConfig _defaultSettingsConfig;
        private readonly IAppConfig _appConfig;

        private IDisposable _disposable;
        private PersistentReactiveProperty<bool> _isSendNotifications;

        internal SettingsRepository(IDefaultSettingsConfig defaultSettingsConfig, IAppConfig appConfig)
        {
            _defaultSettingsConfig = defaultSettingsConfig;
            _appConfig = appConfig;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                LanguageLevel.InitAsync(PersistentKeys.LanguageLevelKey, cancellationToken),
                DailyGoal.InitAsync(PersistentKeys.DailyGoalKey, cancellationToken, _defaultSettingsConfig.DailyGoal),

                CurrentCulture.InitAsync(
                    PersistentKeys.CurrentCultureKey,
                    cancellationToken,
                    CultureInfo.CurrentCulture),

                RepetitionByCooldown.InitAsync(
                    PersistentKeys.RepetitionByCooldownKey,
                    cancellationToken,
                    _defaultSettingsConfig.Cooldowns),

                LanguageByType.InitAsync(
                    PersistentKeys.LanguageByTypeKey,
                    cancellationToken,
                    CreateDefaultLanguageByType()),

                LearningDirection.InitAsync(
                    PersistentKeys.LearningDirectionKey,
                    cancellationToken,
                    LearningDirectionType.LearningToNative),

                CurrentTheme.InitAsync(
                    PersistentKeys.CurrentThemeKey,
                    cancellationToken,
                    AndroidThemeDetector.GetAndroidSystemTheme()),

                IsSendNotifications.InitAsync(
                    PersistentKeys.IsSendNotificationsKey,
                    cancellationToken),

                IsShowTranscription.InitAsync(
                    PersistentKeys.IsShowTranscriptionKey,
                    cancellationToken),

                IsSwipeEnabled.InitAsync(
                    PersistentKeys.IsSwipeEnabledKey,
                    cancellationToken)
            };

            await UniTask.WhenAll(initTasks);

            _disposable = CurrentTheme
                .Subscribe(newTheme => ThemeHandler.Instance.CurrentThemeType.Value = newTheme);
        }

        public void SetLanguage(SystemLanguage newLanguage, LanguageType requestedLanguageType)
        {
            var currentLanguages = LanguageByType.Value;
            var oppositeLanguageType = GetOppositeLanguageType(requestedLanguageType);

            if (currentLanguages[oppositeLanguageType] == newLanguage)
            {
                var previousRequestedLanguage = currentLanguages[requestedLanguageType];
                currentLanguages[oppositeLanguageType] = previousRequestedLanguage;
            }

            currentLanguages[requestedLanguageType] = newLanguage;
            LanguageByType.Property.OnNext(currentLanguages);
        }

        private LanguageType GetOppositeLanguageType(LanguageType languageType) =>
            languageType == LanguageType.Native ? LanguageType.Learning : LanguageType.Native;

        private EnumArray<LanguageType, SystemLanguage> CreateDefaultLanguageByType()
        {
            var nativeLanguage = LocalizationController.Language.Value;

            if (_appConfig.SupportedLanguages[LanguageType.Native].Contains(nativeLanguage) is false)
                nativeLanguage = _appConfig.DefaultNativeLanguage;

            var learningLanguage = _appConfig.DefaultLearningLanguage == nativeLanguage
                ? _appConfig.DefaultNativeLanguage
                : _appConfig.DefaultLearningLanguage;

            var languageArray = new EnumArray<LanguageType, SystemLanguage>(EnumMode.SkipFirst)
            {
                [LanguageType.Native] = nativeLanguage,
                [LanguageType.Learning] = learningLanguage
            };

            return languageArray;
        }

        public void Dispose()
        {
            CurrentCulture.Dispose();
            LearningDirection.Dispose();
            RepetitionByCooldown.Dispose();
            LanguageLevel.Dispose();
            LanguageByType.Dispose();
            _disposable.Dispose();
        }
    }
}