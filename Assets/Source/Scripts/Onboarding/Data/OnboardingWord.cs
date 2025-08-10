using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Core.Sprites;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Onboarding.Data
{
    [Serializable]
    internal struct OnboardingWord
    {
        [field: SerializeField] internal AssetReferenceT<Sprite> DescriptiveImage { get; private set; }
        [field: SerializeField] internal string WordLocalizationKey { get; private set; }
        [field: SerializeField] internal string ExampleWordLocalizationKey { get; private set; }

        internal WordEntry CreateWord(EnumArray<LanguageType, SystemLanguage> selectedLanguages) =>
            new()
            {
                NativeWord = WordLocalizationKey.GetLocalization(selectedLanguages[LanguageType.Native]),
                LearningWord = WordLocalizationKey.GetLocalization(selectedLanguages[LanguageType.Learning]),
                NativeExample = ExampleWordLocalizationKey.GetLocalization(selectedLanguages[LanguageType.Native]),
                LearningExample = ExampleWordLocalizationKey.GetLocalization(selectedLanguages[LanguageType.Learning]),
                DescriptiveImage = new CachedSprite(DescriptiveImage.AssetGUID)
            };
    }
}