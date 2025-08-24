using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.Onboarding.Data
{
    internal sealed class OnboardingConfig : ScriptableObject, IOnboardingConfig
    {
        [field: SerializeField] public List<int> DefaultWordGoals { get; private set; }
    }
}