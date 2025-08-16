using System;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using R3;
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

        internal WordEntry CreateWord(SystemLanguage nativeLanguage, SystemLanguage learningLanguage) =>
            new()
            {
                NativeWord = WordLocalizationKey.GetLocalization(nativeLanguage),
                LearningWord = WordLocalizationKey.GetLocalization(learningLanguage),
                NativeExample = ExampleWordLocalizationKey.GetLocalization(nativeLanguage),
                LearningExample = ExampleWordLocalizationKey.GetLocalization(learningLanguage),
                DescriptiveImage = new CachedSprite(DescriptiveImage.AssetGUID)
            };
    }
}