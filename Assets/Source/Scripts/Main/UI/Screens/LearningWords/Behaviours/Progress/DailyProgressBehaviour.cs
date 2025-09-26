using System;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using CustomUtils.Runtime.UI.CustomComponents;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.Data.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress
{
    internal sealed class DailyProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentProgressPercentText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _progressDescriptionText;
        [SerializeField] private RoundedFilledImageComponent _progressComponent;

        private IProgressDescriptionsDatabase _progressDescriptionsDatabase;
        private ILocalizationKeysDatabase _localizationKeysDatabase;
        private IProgressRepository _progressRepository;

        [Inject]
        internal void Inject(
            IProgressDescriptionsDatabase progressDescriptionsDatabase,
            ILocalizationKeysDatabase localizationKeysDatabase,
            IProgressRepository progressRepository)
        {
            _progressDescriptionsDatabase = progressDescriptionsDatabase;
            _localizationKeysDatabase = localizationKeysDatabase;
            _progressRepository = progressRepository;
        }

        internal void Init()
        {
            _progressRepository.ProgressHistory
                .DistinctUntilChangedBy(progress => progress)
                .SubscribeUntilDestroy(this, static self => self.UpdateProgress());

            LocalizationController.Language.SubscribeUntilDestroy(this, static self => self.UpdateProgress());
        }

        private void UpdateProgress()
        {
            var (learnedCount, dailyGoal) = GetProgressData();
            var progressRatio = (float)learnedCount / dailyGoal;

            UpdateProgressUI(progressRatio);
            UpdateDescriptionUI(learnedCount, dailyGoal);
        }

        private (int learnedCount, int dailyGoal) GetProgressData()
        {
            var progressHistory = _progressRepository.ProgressHistory.CurrentValue;
            var dailyGoal = Mathf.Max(1, _progressRepository.NewWordsDailyTarget.CurrentValue);

            var learnedCount = progressHistory.TryGetValue(DateTime.Now, out var dailyProgress)
                ? Mathf.Max(0, dailyProgress.GetProgressCountData(LearningState.CurrentlyLearning))
                : 0;

            return (learnedCount, dailyGoal);
        }

        private void UpdateProgressUI(float progressRatio)
        {
            var progressPercent = Mathf.RoundToInt(progressRatio * 100);
            var fillAmount = Mathf.Min(progressRatio, 1.0f);

            _currentProgressPercentText.SetTextFormat("{0}%", progressPercent);
            _progressComponent.fillAmount = fillAmount;
        }

        private void UpdateDescriptionUI(int learnedCount, int dailyGoal)
        {
            var progressType = DetermineProgressType(learnedCount);
            var descriptionData = GetDescriptionData(progressType);

            _titleText.text = descriptionData.Title;
            _progressDescriptionText.SetTextFormat(descriptionData.Description,
                learnedCount, dailyGoal, descriptionData.Percent);
        }

        private ProgressDescriptionType DetermineProgressType(int learnedCount)
        {
            if (learnedCount <= 0)
                return ProgressDescriptionType.Zero;

            if (learnedCount < _progressDescriptionsDatabase.LowMediumTransitionRandom.RandomValue)
                return ProgressDescriptionType.Low;

            return learnedCount < _progressDescriptionsDatabase.MediumHighTransitionRandom.RandomValue
                ? ProgressDescriptionType.Medium
                : ProgressDescriptionType.High;
        }

        private DescriptionData GetDescriptionData(ProgressDescriptionType progressType)
        {
            var description = _progressDescriptionsDatabase.Descriptions[progressType];
            var localizations = description.ProgressLocalizationData;

            if (localizations.Count == 0)
                return new DescriptionData(
                    _localizationKeysDatabase.GetLocalization(LocalizationType.ProgressTitle),
                    _localizationKeysDatabase.GetLocalization(LocalizationType.ProgressDescription),
                    _progressDescriptionsDatabase.DefaultRandomPercent.RandomValue);

            var randomData = localizations.Random();
            return new DescriptionData(randomData.TitleKey.GetLocalization(),
                randomData.ProgressDescriptionKey.GetLocalization(),
                description.Percent.RandomValue);
        }
    }
}