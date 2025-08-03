using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Data.Repositories.Settings
{
    internal sealed class SettingsRepository : ISettingsRepository, IRepository
    {
        public PersistentReactiveProperty<LanguageLevel> LanguageLevel { get; } = new();
        public PersistentReactiveProperty<int> DailyGoal { get; } = new();
        public PersistentReactiveProperty<CultureInfo> CurrentCulture { get; } = new();
        public PersistentReactiveProperty<List<CooldownByDate>> RepetitionByCooldown { get; } = new();
        public PersistentReactiveProperty<EnumArray<LanguageType, Language>> LanguageByType { get; } = new();
        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; } = new();

        private Language DefaultLearningLanguage => Language.English;
        private Language AdditionalDefaultLanguage => Language.Russian;

        private static readonly Dictionary<SystemLanguage, Language> _systemLanguageToLanguage =
            new()
            {
                [SystemLanguage.Russian] = Language.Russian,
                [SystemLanguage.English] = Language.English
            };

        private readonly IDefaultSettingsDatabase _defaultSettingsDatabase;

        internal SettingsRepository(IDefaultSettingsDatabase defaultSettingsDatabase)
        {
            _defaultSettingsDatabase = defaultSettingsDatabase;
        }

        public async UniTask InitAsync(CancellationToken cancellationToken)
        {
            var initTasks = new[]
            {
                LanguageLevel.InitAsync(PersistentKeys.LanguageLevelKey, cancellationToken),
                DailyGoal.InitAsync(PersistentKeys.DailyGoalKey, cancellationToken, _defaultSettingsDatabase.DailyGoal),

                CurrentCulture.InitAsync(
                    PersistentKeys.CurrentCultureKey,
                    cancellationToken,
                    CultureInfo.CurrentCulture),

                RepetitionByCooldown.InitAsync(
                    PersistentKeys.RepetitionByCooldownKey,
                    cancellationToken,
                    _defaultSettingsDatabase.Cooldowns),

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

            var languageArray = new EnumArray<LanguageType, Language>(EnumMode.SkipFirst)
            {
                [LanguageType.Native] = systemLanguage,
                [LanguageType.Learning] = DefaultLearningLanguage == systemLanguage
                    ? AdditionalDefaultLanguage
                    : DefaultLearningLanguage,
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