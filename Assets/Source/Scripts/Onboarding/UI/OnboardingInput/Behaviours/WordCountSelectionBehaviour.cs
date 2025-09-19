using CustomUtils.Runtime.Extensions;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Onboarding.Data.Config;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours
{
    internal sealed class WordCountSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private ToggleGroup _toggleGroup;
        [SerializeField] private ToggleComponent _wordCountItem;

        private IPracticeSettingsRepository _practiceSettingsRepository;
        private IOnboardingConfig _onboardingConfig;

        [Inject]
        internal void Inject(
            IPracticeSettingsRepository practiceSettingsRepository,
            IOnboardingConfig onboardingConfig)
        {
            _practiceSettingsRepository = practiceSettingsRepository;
            _onboardingConfig = onboardingConfig;
        }

        internal override void Init()
        {
            foreach (var wordGoal in _onboardingConfig.DefaultWordGoals)
            {
                var createdWordItem = Instantiate(_wordCountItem, _contentContainer);

                createdWordItem.Text.text = wordGoal.ToString();
                createdWordItem.group = _toggleGroup;
                createdWordItem.OnPointerClickAsObservable()
                    .SubscribeAndRegister(this, wordGoal, static (wordGoal, self) => self.SelectWordCount(wordGoal));

                if (_practiceSettingsRepository.DailyGoal.Value == wordGoal)
                    createdWordItem.isOn = true;
            }
        }

        private void SelectWordCount(int wordsGoal)
        {
            _practiceSettingsRepository.DailyGoal.Value = wordsGoal;
            continueSubject.OnNext(Unit.Default);
        }
    }
}