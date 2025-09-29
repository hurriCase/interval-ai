using System.Collections.Generic;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.Theme;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.Onboarding.Data.Config;
using Source.Scripts.Onboarding.UI.OnboardingPractice.Steps.Base;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingPractice
{
    internal sealed class OnboardingPracticePopUp : PopUpBase
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private ThemeComponent _messageTheme;
        [SerializeField] private HintTextMapping _hintTextMapping;

        [SerializeReferenceDropdown, SerializeReference] private List<PracticeStepBase> _practiceSteps;

        [SerializeField] private CardBehaviour _cardBehaviour;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;
        [SerializeField] private ControlButtonsBehaviour _controlButtonsBehaviour;

        private ButtonComponent _continueButton;
        private GameObject _placeholderObject;

        private int _currentStepIndex = -1;

        private IPracticeStateService _practiceStateService;
        private IOnboardingConfig _onboardingConfig;

        [Inject]
        internal void Inject(IPracticeStateService practiceStateService, IOnboardingConfig onboardingConfig)
        {
            _practiceStateService = practiceStateService;
            _onboardingConfig = onboardingConfig;
        }

        internal void SwitchStep(PracticeState practiceState, ModuleType moduleType)
        {
            _practiceStateService.SetState(practiceState);
            _cardBehaviour.SwitchModuleCommand.Execute(moduleType);

            _currentStepIndex++;

            UpdateView();
        }

        internal override void Init()
        {
            var practiceState = _onboardingConfig.OnboardingPracticeState;

            _cardBehaviour.Init(practiceState);
            _controlButtonsBehaviour.Init(practiceState);

            foreach (var wordPracticeStepData in _practiceSteps)
            {
                wordPracticeStepData.Init(_hintTextMapping, destroyCancellationToken);
                wordPracticeStepData.OnSwitched.SubscribeUntilDestroy(this, static self => self.SwitchStep());
            }
        }

        private void SwitchStep()
        {
            var currentStep = _practiceSteps[_currentStepIndex];
            currentStep.HideStep();

            if (currentStep.IsTransition)
            {
                HideAsync().Forget();
                return;
            }

            _currentStepIndex++;

            if (_currentStepIndex >= _practiceSteps.Count)
                return;

            UpdateView();
        }

        private void UpdateView()
        {
            var currentStep = _practiceSteps[_currentStepIndex];
            currentStep.UpdateText(_messageText, _messageTheme);
            currentStep.ActiveStep();
        }

        private void OnDestroy()
        {
            if (_currentStepIndex < 0 || _currentStepIndex >= _practiceSteps.Count)
                return;

            _practiceSteps[_currentStepIndex].HideStep();
        }
    }
}