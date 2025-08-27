using System.Collections.Generic;
using Source.Scripts.Core.Localization.LocalizationTypes;
using UnityEngine;

namespace Source.Scripts.Onboarding.Data.Config
{
    internal sealed class OnboardingConfig : ScriptableObject, IOnboardingConfig
    {
        [field: SerializeField] public PracticeState OnboardingPracticeState { get; private set; }
        [field: SerializeField] public List<int> DefaultWordGoals { get; private set; }
    }
}