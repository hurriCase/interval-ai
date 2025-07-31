using System;
using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI.CustomComponents;
using R3;
using Source.Scripts.Core.DI.Repositories.Progress;
using Source.Scripts.Core.DI.Repositories.Progress.Base;
using Source.Scripts.Core.DI.Repositories.Words.Base;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Main.Data.Base;
using TMPro;
using UnityEngine;
using VContainer;
using ZLinq;

namespace Source.Scripts.Main.UI.Screens.LearningWords.Behaviours.Progress
{
    internal sealed class DailyProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentProgressPercentText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _progressDescriptionText;
        [SerializeField] private RoundedFilledImageComponent _progressComponent;

        [Inject] private IProgressRepository _progressRepository;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private IProgressDescriptionsDatabase _progressDescriptionsDatabase;

        internal void Init()
        {
            _progressRepository.ProgressHistory
                .AsObservable()
                .DistinctUntilChangedBy(progress => progress)
                .Subscribe(this, (progressEntry, behaviour) => behaviour.UpdateProgress(progressEntry));

            UpdateProgress(_progressRepository.ProgressHistory.Value);
        }

        private void UpdateProgress(Dictionary<DateTime, DailyProgress> progressHistory)
        {
            var dailyGoal = Mathf.Max(1, _progressRepository.NewWordsDailyTarget.Value);
            var learnedCount = progressHistory.TryGetValue(DateTime.Now, out var dailyProgress)
                ? Mathf.Max(0, dailyProgress.GetProgressCountData(LearningState.CurrentlyLearning))
                : 0;

            var currentProgressRatio = learnedCount / dailyGoal;

            var displayRatio = Mathf.Min(currentProgressRatio, 1.0f);

            var currentProgressPercent = Mathf.RoundToInt(currentProgressRatio * 100);

            _currentProgressPercentText.text = $"{currentProgressPercent}%";
            _progressComponent.fillAmount = displayRatio;

            var (titleLocalization, progressLocalization, percent) =
                GetRandomDescription(DetermineProgressType(learnedCount));

            _titleText.text = titleLocalization;
            _progressDescriptionText.text = string.Format(progressLocalization, learnedCount, dailyGoal, percent);
        }

        private ProgressDescriptionType DetermineProgressType(int learnedWordCount)
        {
            if (learnedWordCount <= 0)
                return ProgressDescriptionType.Zero;

            if (learnedWordCount < _progressDescriptionsDatabase.LowMediumTransitionRandom.RandomValue)
                return ProgressDescriptionType.Low;

            return learnedWordCount < _progressDescriptionsDatabase.MediumHighTransitionRandom.RandomValue
                ? ProgressDescriptionType.Medium
                : ProgressDescriptionType.High;
        }

        private (string, string, int) GetRandomDescription(ProgressDescriptionType progressType)
        {
            var localizationData =
                _progressDescriptionsDatabase.DescriptionLocalizations.AsValueEnumerable()
                    .Where(progressDescriptionData => progressDescriptionData.Type == progressType);

            if (localizationData.Count() == 0)
                return (_localizationKeysDatabase.GetLocalization(LocalizationType.ProgressTitle),
                    _localizationKeysDatabase.GetLocalization(LocalizationType.ProgressDescription),
                    _progressDescriptionsDatabase.DefaultRandomPercent.RandomValue);

            var randomData = localizationData.Random();

            return (randomData.TitleKey.GetLocalization(),
                randomData.ProgressDescriptionKey.GetLocalization(),
                randomData.Percent.RandomValue);
        }
    }
}