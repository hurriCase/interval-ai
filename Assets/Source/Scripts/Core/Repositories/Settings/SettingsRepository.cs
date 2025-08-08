using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Core.Repositories.Settings
{
    internal sealed class SettingsRepository : ISettingsRepository, IRepository
    {
        public PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; } = new();
        public PersistentReactiveProperty<int> DailyGoal { get; } = new();
        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; } = new();
        public PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; } = new();
        public PersistentReactiveProperty<EnumArray<LanguageType, Language>> LanguageByType { get; } = new();
        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; } = new();

        private static readonly Dictionary<SystemLanguage, Language> _systemLanguageToLanguage =
            new()
            {
                [SystemLanguage.Russian] = Language.Russian,
                [SystemLanguage.English] = Language.English
            };

        private readonly IDefaultSettingsConfig _defaultSettingsConfig;

        internal SettingsRepository(IDefaultSettingsConfig defaultSettingsConfig)
        {
            _defaultSettingsConfig = defaultSettingsConfig;
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
                    LearningDirectionType.LearningToNative)
            };

            await UniTask.WhenAll(initTasks);
        }

        public void SetLanguage(Language language, LanguageType languageType)
        {
            var currentLanguages = LanguageByType.Value;
            var oppositeLanguageType = GetOppositeLanguageType(languageType);

            if (currentLanguages[oppositeLanguageType] == language)
            {
                var previousRequestedLanguage = currentLanguages[languageType];
                currentLanguages[oppositeLanguageType] = previousRequestedLanguage;
            }

            currentLanguages[languageType] = language;

            LanguageByType.Value = currentLanguages;
            LanguageByType.Property.OnNext(currentLanguages);
        }

        private LanguageType GetOppositeLanguageType(LanguageType languageType) =>
            languageType == LanguageType.Native ? LanguageType.Learning : LanguageType.Native;

        private EnumArray<LanguageType, Language> CreateDefaultLanguageByType()
        {
            var systemLanguage = _systemLanguageToLanguage[LocalizationController.Language.Value];

            var learningLanguage = _defaultSettingsConfig.DefaultLearningLanguage == systemLanguage
                ? _defaultSettingsConfig.AdditionalDefaultLanguage
                : _defaultSettingsConfig.DefaultLearningLanguage;

            var languageArray = new EnumArray<LanguageType, Language>(EnumMode.SkipFirst)
            {
                [LanguageType.Native] = systemLanguage,
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
        }
    }
}