using System.Collections.Generic;

namespace Source.Scripts.Onboarding.Data
{
    internal interface IOnboardingConfig
    {
        List<int> DefaultWordGoals { get; }
    }
}