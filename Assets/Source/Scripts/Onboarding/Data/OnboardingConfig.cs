using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.Scripts.Onboarding.Data
{
    internal sealed class OnboardingConfig : ScriptableObject, IOnboardingConfig
    {
        [field: SerializeField] public OnboardingWord OnboardingWord { get; private set; }
        [field: SerializeField] public List<int> DefaultWordGoals { get; private set; }
    }

    [Serializable]
    internal struct OnboardingWord
    {
        [field: SerializeField] internal AssetReferenceT<Sprite> DescriptiveImage { get; private set; }
        [field: SerializeField] internal string NativeWordLocalizationKey { get; private set; }
        [field: SerializeField] internal string LearningWordLocalizationKey { get; private set; }
        [field: SerializeField] internal string NativeExampleWordLocalizationKey { get; private set; }
        [field: SerializeField] internal string LearningExampleWordLocalizationKey { get; private set; }

        internal WordEntry CreateWord() =>
            new()
            {
                NativeWord = NativeWordLocalizationKey.GetLocalization(),
                LearningWord = LearningWordLocalizationKey.GetLocalization(),
                NativeExample = NativeExampleWordLocalizationKey.GetLocalization(),
                LearningExample = LearningExampleWordLocalizationKey.GetLocalization(),
                DescriptiveImage = new CachedSprite(DescriptiveImage.AssetGUID)
            };
    }
}