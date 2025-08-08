using System.Collections.Generic;

namespace Source.Scripts.Onboarding.Data
{
    internal interface IOnboardingConfig
    {
        OnboardingWord OnboardingWord { get; }
        List<int> DefaultWordGoals { get; }
    }
}