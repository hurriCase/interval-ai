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
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Settings
{
    internal sealed class SettingsRepository : ISettingsRepository, ILoadable, IDisposable
    {
        public PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; } = new();
        public PersistentReactiveProperty<int> DailyGoal { get; } = new();
        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; } = new();
        public PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; } = new();
        public PersistentReactiveProperty<SystemLanguage> SystemLanguage { get; } = new();
        public PersistentReactiveProperty<LanguageType> FirstShowPractice { get; } = new();
        public PersistentReactiveProperty<LanguageType> CardLearnPractice { get; } = new();
        public PersistentReactiveProperty<LanguageType> CardReviewPractice { get; } = new();
        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; } = new();
        public PersistentReactiveProperty<ThemeType> ThemeType { get; } = new();
        public PersistentReactiveProperty<WordReviewSourceType> WordReviewSourceType { get; } = new();

        public PersistentReactiveProperty<bool> IsSendNotifications { get; } = new();
        public PersistentReactiveProperty<bool> IsShowTranscription { get; } = new();
        public PersistentReactiveProperty<bool> IsSwipeEnabled { get; } = new();

        public PersistentReactiveProperty<EnumArray<LanguageType, ReactiveProperty<SystemLanguage>>> LanguageByType
        {
            get;
        } = new();

        private readonly IDefaultSettingsConfig _defaultSettingsConfig;
        private readonly IAppConfig _appConfig;

        private DisposableBag _disposableBag;

        internal SettingsRepository(IDefaultSettingsConfig defaultSettingsConfig, IAppConfig appConfig)
        {
            _defaultSettingsConfig = defaultSettingsConfig;
            _appConfig = appConfig;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                DailyGoal.InitAsync(PersistentKeys.DailyGoalKey, cancellationToken, _defaultSettingsConfig.DailyGoal),

                LanguageLevel.InitAsync(
                    PersistentKeys.LanguageLevelKey,
                    cancellationToken,
                    _defaultSettingsConfig.LanguageLevel),

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

                SystemLanguage.InitAsync(
                    PersistentKeys.SystemLanguageKey,
                    cancellationToken,
                    GetNativeLanguage()),

                FirstShowPractice.InitAsync(
                    PersistentKeys.FirstShowLanguageKey,
                    cancellationToken,
                    _defaultSettingsConfig.FirstShowPractice),

                CardLearnPractice.InitAsync(
                    PersistentKeys.CardLearnLanguageKey,
                    cancellationToken,
                    _defaultSettingsConfig.CardLearnPractice),

                CardReviewPractice.InitAsync(
                    PersistentKeys.CardReviewLanguageKey,
                    cancellationToken,
                    _defaultSettingsConfig.CardReviewPractice),

                LearningDirection.InitAsync(
                    PersistentKeys.LearningDirectionKey,
                    cancellationToken,
                    LearningDirectionType.LearningToNative),

                ThemeType.InitAsync(
                    PersistentKeys.CurrentThemeKey,
                    cancellationToken,
                    AndroidThemeDetector.GetAndroidSystemTheme()),

                WordReviewSourceType.InitAsync(
                    PersistentKeys.WordReviewSourceTypeKey,
                    cancellationToken,
                    _defaultSettingsConfig.WordReviewSourceType),

                IsSendNotifications.InitAsync(PersistentKeys.IsSendNotificationsKey, cancellationToken),
                IsShowTranscription.InitAsync(PersistentKeys.IsShowTranscriptionKey, cancellationToken),
                IsSwipeEnabled.InitAsync(PersistentKeys.IsSwipeEnabledKey, cancellationToken)
            };
            await UniTask.WhenAll(initTasks);

            SubscribeToChanges();
        }

        private void SubscribeToChanges()
        {
            ThemeType
                .Subscribe(newTheme => ThemeHandler.Instance.CurrentThemeType.Value = newTheme)
                .AddTo(ref _disposableBag);

            SystemLanguage
                .Subscribe(newLanguage => LocalizationController.Language.Value = newLanguage)
                .AddTo(ref _disposableBag);
        }

        public void SetLanguage(SystemLanguage newLanguage, LanguageType requestedLanguageType)
        {
            var currentLanguages = LanguageByType.Value;
            var oppositeLanguageType = GetOppositeLanguageType(requestedLanguageType);

            if (currentLanguages[oppositeLanguageType].Value == newLanguage)
            {
                var previousRequestedLanguage = currentLanguages[requestedLanguageType];
                currentLanguages[oppositeLanguageType] = previousRequestedLanguage;
            }

            currentLanguages[requestedLanguageType].Value = newLanguage;
            LanguageByType.Property.OnNext(currentLanguages);
        }

        private LanguageType GetOppositeLanguageType(LanguageType languageType) =>
            languageType == LanguageType.Native ? LanguageType.Learning : LanguageType.Native;

        private EnumArray<LanguageType, ReactiveProperty<SystemLanguage>> CreateDefaultLanguageByType()
        {
            var nativeLanguage = GetNativeLanguage();

            var learningLanguage = _defaultSettingsConfig.LearningLanguage == nativeLanguage
                ? _defaultSettingsConfig.NativeLanguage
                : _defaultSettingsConfig.LearningLanguage;

            var languageArray = new EnumArray<LanguageType, ReactiveProperty<SystemLanguage>>(
                () => new ReactiveProperty<SystemLanguage>(),
                EnumMode.SkipFirst);

            languageArray[LanguageType.Native].Value = nativeLanguage;
            languageArray[LanguageType.Learning].Value = learningLanguage;

            return languageArray;
        }

        private SystemLanguage GetNativeLanguage()
        {
            var nativeLanguage = LocalizationController.Language.Value;

            return _appConfig.SupportedLanguages[LanguageType.Native].Contains(nativeLanguage)
                ? nativeLanguage
                : _defaultSettingsConfig.NativeLanguage;
        }

        public void Dispose()
        {
            _disposableBag.Dispose();
            LanguageLevel.Dispose();
            DailyGoal.Dispose();
            CurrentCulture.Dispose();
            RepetitionByCooldown.Dispose();
            SystemLanguage.Dispose();
            FirstShowPractice.Dispose();
            CardLearnPractice.Dispose();
            CardReviewPractice.Dispose();
            LearningDirection.Dispose();
            ThemeType.Dispose();
            WordReviewSourceType.Dispose();
            IsSendNotifications.Dispose();
            IsShowTranscription.Dispose();
            IsSwipeEnabled.Dispose();
            LanguageByType.Dispose();
        }
    }
}