using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Onboarding.UI.Base;
using Source.Scripts.Onboarding.UI.WordPractice;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.Setup.Behaviours
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

        internal override void HandleContinue()
        {
            var onboardingPracticePopUp = _windowsController.OpenPopUp<WordPracticePopUp>();
            onboardingPracticePopUp.SwitchStep(_practiceState, _moduleType);
        }
    }
}