using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Scenes.Base;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Buttons;
using Cysharp.Threading.Tasks;
using R3;
using Source.Scripts.Core.References.Base;
using Source.Scripts.Core.Repositories.Statistics;
using Source.Scripts.Onboarding.UI.Setup.Behaviours;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.Setup
{
    internal sealed class SetupScreen : ScreenBase
    {
        [SerializeField] private ThemeButton _continueButton;

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
                inputOnboardingStep.OnNextStep.SubscribeUntilDestroy(this, static self => self.SwitchModule().Forget());
            }

            SwitchSettingsStep(_currentStepIndex, true).Forget();

            _continueButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.SwitchModule().Forget());
        }

        private async UniTask SwitchModule()
        {
            var nextSTep = _currentStepIndex + 1;
            if (nextSTep >= _inputOnboardingSteps.Count)
            {
                _statisticsRepository.IsCompleteOnboarding.Value = true;
                _sceneTransitionController.StartTransition(_sceneReferences.Splash.Address,
                    _sceneReferences.Main.Address).Forget();

                return;
            }

            await SwitchSettingsStep(_currentStepIndex, false);

            _currentStepIndex++;

            SwitchSettingsStep(_currentStepIndex, true).Forget();
        }

        private async UniTask SwitchSettingsStep(int index, bool isActive)
        {
            var inputStep = _inputOnboardingSteps[index];

            if (isActive)
                inputStep.UpdateView();
            else
                await inputStep.HandleContinue();

            inputStep.SetActive(isActive);
        }
    }
}