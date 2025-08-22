using System.Collections.Generic;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.Onboarding.Data;
using Source.Scripts.Onboarding.UI.OnboardingPractice.Steps.Base;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.PopUp;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingPractice
{
    internal sealed class OnboardingPracticePopUp : PopUpBase
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private HintTextMapping _hintTextMapping;

        [SerializeReferenceDropdown, SerializeReference] private List<PracticeStepBase> _practiceSteps;

        [SerializeField] private CardBehaviour _cardBehaviour;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;
        [SerializeField] private ControlButtonsBehaviour _controlButtonsBehaviour;

        [Inject] private ILanguageSettingsRepository _languageSettingsRepository;
        [Inject] private IOnboardingConfig _onboardingConfig;
        [Inject] private IWordsRepository _wordsRepository;
        [Inject] private IAppConfig _appConfig;

        private ButtonComponent _continueButton;
        private GameObject _placeholderObject;

        private int _currentStepIndex = -1;

        internal void SwitchStep(ModuleType moduleType)
        {
            _cardBehaviour.SwitchModuleCommand.Execute(moduleType);

            _currentStepIndex++;

            UpdateView();
        }

        internal override void Init()
        {
            var practiceState = _appConfig.OnboardingPracticeState;

            var onboardingWord = _onboardingConfig.OnboardingWord.CreateWord(
                GetLanguageByType(LanguageType.Native),
                GetLanguageByType(LanguageType.Learning));

            _wordsRepository.SetCurrentWord(practiceState, onboardingWord);

            _cardBehaviour.Init(practiceState);

            _wordProgressBehaviour.Init();
            _controlButtonsBehaviour.Init(practiceState);

            foreach (var wordPracticeStepData in _practiceSteps)
            {
                wordPracticeStepData.Init(_hintTextMapping, destroyCancellationToken);
                wordPracticeStepData.SwitchObservable.SubscribeAndRegister(this, static self => self.SwitchStep());
            }
        }

        private SystemLanguage GetLanguageByType(LanguageType languageType)
            => _languageSettingsRepository.LanguageByType.CurrentValue[languageType];

        private void SwitchStep()
        {
            var currentStep = _practiceSteps[_currentStepIndex];
            currentStep.HideStep();

            if (currentStep.IsTransition)
            {
                Hide();
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
            currentStep.UpdateText(_messageText);
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