using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Randoms;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI;
using Source.Scripts.Data;
using Source.Scripts.Data.Entries;
using Source.Scripts.UI.Localization;
using TMPro;
using UnityEngine;
using ZLinq;

namespace Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Progress
{
    internal sealed class ProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentProgressPercentText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _progressDescriptionText;
        [SerializeField] private RoundedFilledImageComponent _progressComponent;

        [SerializeField] private List<ProgressDescriptionData> _progressDescriptionLocalizations;
        [SerializeField] private RandomInt _lowMediumTransitionRandom;
        [SerializeField] private RandomInt _mediumHighTransitionRandom;
        [SerializeField] private RandomInt _defaultRandomPercent = new(20, 50);

        internal void Init()
        {
            UserData.Instance.ProgressEntry.Subscribe(this,
                (behaviour, property) => behaviour.UpdateProgress(property));
            UpdateProgress(UserData.Instance.ProgressEntry.Value);
        }

        private void UpdateProgress(ProgressEntry progressEntry)
        {
            var dailyGoal = Mathf.Max(1, progressEntry.DailyWordGoal);
            var learnedCount = Mathf.Max(0, progressEntry.TodayLearnedWordCount);

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

            if (learnedWordCount < _lowMediumTransitionRandom.RandomValue)
                return ProgressDescriptionType.Low;

            return learnedWordCount < _mediumHighTransitionRandom.RandomValue
                ? ProgressDescriptionType.Medium
                : ProgressDescriptionType.High;
        }

        private (string, string, int) GetRandomDescription(ProgressDescriptionType progressType)
        {
            var localizationData = _progressDescriptionLocalizations.AsValueEnumerable()
                .Where(progressDescriptionData => progressDescriptionData.Type == progressType);

            if (localizationData.Count() == 0)
                return (LocalizationType.ProgressTitle.GetLocalization(),
                    LocalizationType.ProgressDescription.GetLocalization(),
                    _defaultRandomPercent.RandomValue);

            var randomData = localizationData.Random();

            return (randomData.TitleKey.GetLocalization(),
                randomData.ProgressDescriptionKey.GetLocalization(),
                randomData.Percent.RandomValue);
        }
    }
}