using System;
using System.Linq;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Settings.Repositories
{
    internal sealed class LanguageSettingsRepository : ILanguageSettingsRepository, IRepository, IDisposable
    {
        public PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; } = new();
        public PersistentReactiveProperty<SystemLanguage> SystemLanguage { get; } = new();
        public PersistentReactiveProperty<LanguageType> FirstShowLanguageType { get; } = new();
        public PersistentReactiveProperty<LanguageType> CardLearnLanguageType { get; } = new();
        public PersistentReactiveProperty<LanguageType> CardReviewLanguageType { get; } = new();

        public ReadOnlyReactiveProperty<EnumArray<LanguageType, SystemLanguage>> LanguageByType
            => _languageByType.Property;

        public EnumArray<LanguageType, ReactiveProperty<SystemLanguage>> LanguageProperties { get; }
            = new(() => new ReactiveProperty<SystemLanguage>(), EnumMode.SkipFirst);

        private readonly PersistentReactiveProperty<EnumArray<LanguageType, SystemLanguage>> _languageByType = new();

        private readonly IDefaultSettingsConfig _defaultSettingsConfig;
        private readonly IAppConfig _appConfig;

        private DisposableBag _disposableBag;

        internal LanguageSettingsRepository(IDefaultSettingsConfig defaultSettingsConfig, IAppConfig appConfig)
        {
            _defaultSettingsConfig = defaultSettingsConfig;
            _appConfig = appConfig;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                LanguageLevel.InitAsync(
                    PersistentKeys.LanguageLevelKey,
                    cancellationToken,
                    _defaultSettingsConfig.LanguageLevel),

                _languageByType.InitAsync(
                    PersistentKeys.LanguageByTypeKey,
                    cancellationToken,
                    CreateDefaultLanguageByType()),

                SystemLanguage.InitAsync(
                    PersistentKeys.SystemLanguageKey,
                    cancellationToken,
                    GetNativeLanguage()),

                FirstShowLanguageType.InitAsync(
                    PersistentKeys.FirstShowLanguageKey,
                    cancellationToken,
                    _defaultSettingsConfig.FirstShowPractice),

                CardLearnLanguageType.InitAsync(
                    PersistentKeys.CardLearnLanguageKey,
                    cancellationToken,
                    _defaultSettingsConfig.CardLearnPractice),

                CardReviewLanguageType.InitAsync(
                    PersistentKeys.CardReviewLanguageKey,
                    cancellationToken,
                    _defaultSettingsConfig.CardReviewPractice),
            };

            await UniTask.WhenAll(initTasks);

            SystemLanguage
                .Subscribe(newLanguage => LocalizationController.Language.Value = newLanguage)
                .AddTo(ref _disposableBag);

            MapLanguageProperties();
        }

        private void MapLanguageProperties()
        {
            foreach (var (languageType, systemLanguage) in _languageByType.Value.AsTuples())
                LanguageProperties[languageType].Value = systemLanguage;

            foreach (var (languageType, property) in LanguageProperties.AsTuples())
            {
                property.Subscribe((self: this, languageType), static (language, tuple)
                        => tuple.self.SetLanguage(language, tuple.languageType))
                    .AddTo(ref _disposableBag);
            }
        }

        public void SetLanguage(SystemLanguage newLanguage, LanguageType requestedLanguageType)
        {
            var currentLanguages = _languageByType.Value;
            var oppositeLanguageType = GetOppositeLanguageType(requestedLanguageType);

            if (currentLanguages[oppositeLanguageType] == newLanguage)
            {
                var previousRequestedLanguage = currentLanguages[requestedLanguageType];
                currentLanguages[oppositeLanguageType] = previousRequestedLanguage;
            }

            currentLanguages[requestedLanguageType] = newLanguage;
            _languageByType.Property.OnNext(currentLanguages);
        }

        private LanguageType GetOppositeLanguageType(LanguageType languageType) =>
            languageType == LanguageType.Native ? LanguageType.Learning : LanguageType.Native;

        private EnumArray<LanguageType, SystemLanguage> CreateDefaultLanguageByType()
        {
            var nativeLanguage = GetNativeLanguage();

            var learningLanguage = _defaultSettingsConfig.LearningLanguage == nativeLanguage
                ? _defaultSettingsConfig.NativeLanguage
                : _defaultSettingsConfig.LearningLanguage;

            var defaultLanguages = new EnumArray<LanguageType, SystemLanguage>(EnumMode.SkipFirst)
            {
                [LanguageType.Native] = nativeLanguage,
                [LanguageType.Learning] = learningLanguage
            };

            return defaultLanguages;
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
            _languageByType.Dispose();
            _disposableBag.Dispose();
            LanguageLevel.Dispose();
            SystemLanguage.Dispose();
            FirstShowLanguageType.Dispose();
            CardLearnLanguageType.Dispose();
            CardReviewLanguageType.Dispose();
        }
    }
}