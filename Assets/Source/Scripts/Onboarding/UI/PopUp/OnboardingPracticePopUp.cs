using System.Collections.Generic;
using R3;
using Source.Scripts.Core.Configs;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours;
using Source.Scripts.Onboarding.Data;
using Source.Scripts.Onboarding.UI.PopUp.WordPractice;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base.PopUp;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Onboarding.UI.PopUp
{
    internal sealed class OnboardingPracticePopUp : PopUpBase
    {
        [SerializeField] private TextMeshProUGUI _messageText;

        [SerializeField] private Transform _imageTint;
        [SerializeReferenceDropdown, SerializeReference] private List<PracticeStepBase> _practiceSteps;

        [SerializeField] private CardBehaviour _cardBehaviour;
        [SerializeField] private WordProgressBehaviour _wordProgressBehaviour;
        [SerializeField] private ControlButtonsBehaviour _controlButtonsBehaviour;

        [Inject] private IOnboardingConfig _onboardingConfig;
        [Inject] private ISettingsRepository _settingsRepository;
        [Inject] private IWordsRepository _wordsRepository;
        [Inject] private IAppConfig _appConfig;

        private ButtonComponent _continueButton;
        private GameObject _placeholderObject;

        private int _currentStepIndex;

        private bool _isPlainSteps;

        internal override void Init()
        {
            var practiceState = _appConfig.OnboardingPracticeState;
            _cardBehaviour.Init(practiceState);

            var onboardingWord = _onboardingConfig.OnboardingWord.CreateWord(
                GetLanguageByType(LanguageType.Native),
                GetLanguageByType(LanguageType.Learning));

            _wordsRepository.SetCurrentWord(practiceState, onboardingWord);

            _wordProgressBehaviour.Init();
            _controlButtonsBehaviour.Init(practiceState);

            foreach (var wordPracticeStepData in _practiceSteps)
            {
                wordPracticeStepData.Init(_imageTint, destroyCancellationToken);
                wordPracticeStepData.ButtonClickObservable.Subscribe(this,
                        static (_, self) => self.SwitchStep())
                    .RegisterTo(destroyCancellationToken);
            }

            UpdateView();
        }

        private SystemLanguage GetLanguageByType(LanguageType languageType)
            => _settingsRepository.LanguageByType.Value[languageType].Value;

        private void SwitchStep()
        {
            var currentStep = _practiceSteps[_currentStepIndex];
            currentStep.HideStep();

            _currentStepIndex++;

            if (currentStep.IsTransition)
                Hide();

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
            _practiceSteps[_currentStepIndex].HideStep();
        }
    }
}