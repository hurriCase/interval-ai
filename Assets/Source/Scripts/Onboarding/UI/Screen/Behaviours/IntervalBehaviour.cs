using Source.Scripts.Core.Configs;
using Source.Scripts.Onboarding.UI.Base;
using Source.Scripts.Onboarding.UI.PopUp;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.Screen.Behaviours
{
    internal sealed class IntervalBehaviour : StepBehaviourBase
    {
        [SerializeField] private ModuleType _moduleType;

        [Inject] private IWindowsController _windowsController;

        internal override void OnContinue()
        {
            var onboardingPracticePopUp = _windowsController.OpenPopUp<OnboardingPracticePopUp>();
            onboardingPracticePopUp.SetPracticeState(_moduleType);
        }
    }
}