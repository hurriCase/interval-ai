using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public PersistentReactiveProperty<EnumArray<LanguageType, SystemLanguage>> LanguageByType { get; } = new();
        public PersistentReactiveProperty<LearningDirectionType> LearningDirection { get; } = new();

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

        public void SetLanguage(SystemLanguage newLanguage, LanguageType requistedLanguageType)
        {
            var currentLanguages = LanguageByType.Value;
            var oppositeLanguageType = GetOppositeLanguageType(requistedLanguageType);

            if (currentLanguages[oppositeLanguageType] == newLanguage)
            {
                var previousRequestedLanguage = currentLanguages[requistedLanguageType];
                currentLanguages[oppositeLanguageType] = previousRequestedLanguage;
            }

            currentLanguages[requistedLanguageType] = newLanguage;
            LanguageByType.Property.OnNext(currentLanguages);
        }

        private LanguageType GetOppositeLanguageType(LanguageType languageType) =>
            languageType == LanguageType.Native ? LanguageType.Learning : LanguageType.Native;

        private EnumArray<LanguageType, SystemLanguage> CreateDefaultLanguageByType()
        {
            var nativeLanguage = LocalizationController.Language.Value;

            if (_defaultSettingsConfig.SupportedLanguages[LanguageType.Native].Contains(nativeLanguage) is false)
                nativeLanguage = _defaultSettingsConfig.DefaultNativeLanguage;

            var learningLanguage = _defaultSettingsConfig.DefaultLearningLanguage == nativeLanguage
                ? _defaultSettingsConfig.DefaultNativeLanguage
                : _defaultSettingsConfig.DefaultLearningLanguage;

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
        }
    }
}