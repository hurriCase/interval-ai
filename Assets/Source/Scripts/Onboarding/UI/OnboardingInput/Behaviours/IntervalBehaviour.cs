using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Onboarding.UI.Base;
using Source.Scripts.Onboarding.UI.OnboardingPractice;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours
{
    internal sealed class IntervalBehaviour : StepBehaviourBase
    {
        [SerializeField] private PracticeState _practiceState;
        [SerializeField] private ModuleType _moduleType;

        private IWindowsController _windowsController;

        [Inject]
        internal void Inject(IWindowsController windowsController)
        {
            _windowsController = windowsController;
        }

        internal override void OnContinue()
        {
            var onboardingPracticePopUp = _windowsController.OpenPopUp<OnboardingPracticePopUp>();
            onboardingPracticePopUp.SwitchStep(_practiceState, _moduleType);
        }
    }
}