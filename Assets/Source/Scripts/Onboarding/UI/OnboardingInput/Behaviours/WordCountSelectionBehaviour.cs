using CustomUtils.Runtime.Extensions;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Onboarding.Data.Config;
using Source.Scripts.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Onboarding.UI.OnboardingInput.Behaviours
{
    internal sealed class WordCountSelectionBehaviour : StepBehaviourBase
    {
        [SerializeField] private RectTransform _contentContainer;
        [SerializeField] private ToggleGroup _toggleGroup;
        [SerializeField] private RectTransform _rowItem;
        [SerializeField] private ToggleComponent _wordCountItem;
        [SerializeField] private AspectRatioFitter _spacing;
        [SerializeField] private float _rowSpacingRatio;
        [SerializeField] private float _wordCountSpacingRatio;

        private IPracticeSettingsRepository _practiceSettingsRepository;
        private IOnboardingConfig _onboardingConfig;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(
            IPracticeSettingsRepository practiceSettingsRepository,
            IOnboardingConfig onboardingConfig,
            IObjectResolver objectResolver)
        {
            _practiceSettingsRepository = practiceSettingsRepository;
            _onboardingConfig = onboardingConfig;
            _objectResolver = objectResolver;
        }

        private const int WordCountPerRow = 3;

        internal override void Init()
        {
            CreateWordGoalSelections();
        }

        private void CreateWordGoalSelections()
        {
            var wordGoals = _onboardingConfig.DefaultWordGoals;
            var currentRow = new RectTransform();
            for (var i = 0; i < wordGoals.Count; i++)
            {
                if (i % WordCountPerRow == 0)
                {
                    currentRow = _objectResolver.Instantiate(_rowItem, _contentContainer);

                    _spacing.CreateHeightSpacing(_rowSpacingRatio, _contentContainer);
                }

                CreateWordItem(currentRow, wordGoals[i]);
            }
        }

        private void CreateWordItem(Transform currentRow, int wordGoal)
        {
            var createdWordItem = Instantiate(_wordCountItem, currentRow);

            createdWordItem.Text.text = wordGoal.ToString();
            createdWordItem.group = _toggleGroup;
            createdWordItem.OnPointerClickAsObservable()
                .SubscribeAndRegister(this, wordGoal, static (wordGoal, self) => self.SelectWordCount(wordGoal));

            if (wordGoal == _practiceSettingsRepository.DailyGoal.Value)
                createdWordItem.OnEnableAsObservable()
                    .SubscribeAndRegister(this, (createdWordItem, wordGoal), static (tuple, self) =>
                    {
                        if (tuple.wordGoal == self._practiceSettingsRepository.DailyGoal.Value)
                            tuple.createdWordItem.isOn = true;
                    });
        }

        private void SelectWordCount(int wordsGoal)
        {
            _practiceSettingsRepository.DailyGoal.Value = wordsGoal;
            continueSubject.OnNext(Unit.Default);
        }
    }
}