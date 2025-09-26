using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Scenes.Base;
using R3;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Statistics;
using Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput
{
    internal sealed class OnboardingInputScreen : ScreenBase
    {
        [SerializeField] private ButtonComponent _continueButton;

        [SerializeField] private List<StepBehaviourBase> _inputOnboardingSteps;

        private int _currentStepIndex;
        private bool _isWordPracticeOnBoarding;

        private ISceneTransitionController _sceneTransitionController;
        private IStatisticsRepository _statisticsRepository;
        private ISceneReferences _sceneReferences;

        [Inject]
        internal void Inject(
            ISceneTransitionController sceneTransitionController,
            IStatisticsRepository statisticsRepository,
            ISceneReferences sceneReferences)
        {
            _sceneTransitionController = sceneTransitionController;
            _statisticsRepository = statisticsRepository;
            _sceneReferences = sceneReferences;
        }

        internal override void Init()
        {
            foreach (var inputOnboardingStep in _inputOnboardingSteps)
            {
                inputOnboardingStep.SetActive(false);
                inputOnboardingStep.Init();
                inputOnboardingStep.OnContinue.SubscribeUntilDestroy(this, static self => self.SwitchModule());
            }

            SwitchSettingsStep(_currentStepIndex, true);

            _continueButton.OnClickAsObservable().SubscribeUntilDestroy(this, static self => self.SwitchModule());
        }

        private void SwitchModule()
        {
            var nextSTep = _currentStepIndex + 1;
            if (nextSTep >= _inputOnboardingSteps.Count)
            {
                _statisticsRepository.IsCompleteOnboarding.Value = true;
                _sceneTransitionController.StartTransition(_sceneReferences.Splash.Address,
                    _sceneReferences.MainMenuScene.Address);

                return;
            }

            SwitchSettingsStep(_currentStepIndex, false);

            _currentStepIndex++;

            SwitchSettingsStep(_currentStepIndex, true);
        }

        private void SwitchSettingsStep(int index, bool isActive)
        {
            var inputStep = _inputOnboardingSteps[index];
            if (isActive)
                inputStep.UpdateView();
            else
                inputStep.HandleContinue();

            inputStep.SetActive(isActive);
        }
    }
}