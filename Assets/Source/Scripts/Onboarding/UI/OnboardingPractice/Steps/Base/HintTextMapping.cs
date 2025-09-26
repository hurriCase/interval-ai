using CustomUtils.Runtime.UI.Theme;
using Source.Scripts.Core.Others;
using UnityEngine;

namespace Source.Scripts.Onboarding.UI.OnboardingPractice.Steps.Base
{
    [CreateAssetMenu(
        fileName = nameof(HintTextMapping),
        menuName = MenuPaths.MappingsPath + nameof(HintTextMapping)
    )]
    internal sealed class HintTextMapping : ThemeStateMappingGeneric<HintTextThemeState> { }
}