using System.Collections.Generic;
using Source.Scripts.Core.Localization.LocalizationTypes;

namespace Source.Scripts.Onboarding.Data.Config
{
    internal interface IOnboardingConfig
    {
        PracticeState OnboardingPracticeState { get; }
        List<int> DefaultWordGoals { get; }
    }
}