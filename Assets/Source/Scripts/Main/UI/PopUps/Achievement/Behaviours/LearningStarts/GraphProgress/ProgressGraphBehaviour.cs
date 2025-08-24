using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Localization.LocalizationTypes.Date;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.Data.Base;
using Source.Scripts.Main.UI.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal sealed class ProgressGraphBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _maxProgressText;

        [SerializeField] private RectTransform _graphButtonsContainer;
        [SerializeField] private ToggleGroup _graphButtonsGroup;
        [SerializeField] private GraphTypeItem _graphTypeItemPrefab;

        [SerializeField] private AspectRatioFitter _spacingPrefab;
        [SerializeField] private float _spacingRatio;

        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private UILineRenderer _uiLineRenderer;

        [SerializeField] private EnumArray<LearningState, UILineRenderer> _graphLines = new(EnumMode.SkipFirst);

        [Inject] private IDateProgressService _dateProgressService;
        [Inject] private ILocalizationKeysDatabase _localizationKeysDatabase;
        [Inject] private IProgressGraphSettings _progressGraphSettings;

        private readonly Dictionary<LearningState, List<GraphProgressData>> _cashedAllProgressData = new();
        private readonly List<Vector2> _cashedNormalizedPoints = new();

        internal void Init()
        {
            foreach (var dateRange in _progressGraphSettings.GraphProgressRanges)
            {
                var createdGraphType = Instantiate(_graphTypeItemPrefab, _graphButtonsContainer);
                createdGraphType.TabComponent.group = _graphButtonsGroup;
                createdGraphType.TabComponent.OnValueChangedAsObservable()
                    .SubscribeAndRegister(this, dateRange, static (dateRange, self) => self.UpdateGraph(dateRange));

                var createdSpacing = Instantiate(_spacingPrefab, _graphButtonsContainer);
                createdSpacing.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
                createdSpacing.aspectRatio = _spacingRatio;

                LocalizationController.Language
                    .SubscribeAndRegister(this, (dateRange, createdGraphType.Text),
                        static (tuple, self) => self.UpdateLocalization(tuple.dateRange, tuple.Text));
            }
        }

        private void UpdateLocalization(DateRange dateRange, TMP_Text graphTypeText)
        {
            var localizationKey
                = _localizationKeysDatabase.GetDateLocalization(dateRange.DateType, dateRange.Amount);

            graphTypeText.SetText(localizationKey, dateRange.Amount);
        }

        private void UpdateGraph(DateRange progressRange)
        {
            var maxProgress = GenerateAllGraphPoints(progressRange);
            _maxProgressText.text = maxProgress.ToString();

            foreach (var (learningState, uiLineRenderer) in _graphLines.AsTuples())
            {
                uiLineRenderer.color = _progressColorMapping.GetColorForState(learningState);

                var normalizedPoints = NormalizePoints(
                    _cashedAllProgressData[learningState],
                    maxProgress,
                    _progressGraphSettings.GraphPointsCount);

                uiLineRenderer.SetPoints(normalizedPoints);
            }
        }

        private int GenerateAllGraphPoints(DateRange progressRange)
        {
            var totalDays = GetTotalDays(progressRange);
            var pointsCount = _progressGraphSettings.GraphPointsCount;
            var daysPerSegment = (float)totalDays / pointsCount;
            var maxProgress = 0;

            foreach (var (learningState, _) in _graphLines.AsTuples())
            {
                if (_cashedAllProgressData.TryGetValue(learningState, out var progressPoints) is false)
                {
                    progressPoints = new List<GraphProgressData>(pointsCount);
                    _cashedAllProgressData[learningState] = progressPoints;
                }

                progressPoints.Clear();

                for (var i = 0; i < pointsCount; i++)
                {
                    var segmentIndex = pointsCount - 1 - i;
                    var segmentStart = (int)(daysPerSegment * segmentIndex);
                    var segmentEnd = (int)(daysPerSegment * (segmentIndex + 1));
                    var segmentDuration = Math.Max(1, segmentEnd - segmentStart);

                    var progress =
                        _dateProgressService.GetProgressForRange(segmentStart, segmentDuration, learningState);
                    progressPoints.Add(new GraphProgressData(i, progress));
                    maxProgress = Math.Max(maxProgress, progress);
                }
            }

            return maxProgress;
        }

        private List<Vector2> NormalizePoints(List<GraphProgressData> points, int maxProgress, int maxIndex)
        {
            _cashedNormalizedPoints.Clear();
            foreach (var (index, progress) in points)
            {
                var normalizedX = (float)index / (maxIndex - 1);
                var normalizedY = maxProgress > 0 ? (float)progress / maxProgress : 0f;

                _cashedNormalizedPoints.Add(new Vector2(normalizedX, normalizedY));
            }

            return _cashedNormalizedPoints;
        }

        private int GetTotalDays(DateRange progressRange)
        {
            var totalDays = 0;

            if (progressRange.DateType == DateType.Days)
                return progressRange.Amount;

            for (var i = 0; i < progressRange.Amount; i++)
            {
                totalDays += progressRange.DateType switch
                {
                    DateType.Months => GetDaysInMonth(i),
                    DateType.Years => GetDaysInYear(i),
                    _ => throw new ArgumentOutOfRangeException(nameof(progressRange.DateType),
                        progressRange.DateType,
                        "[ProgressGraphBehaviour::GetTotalDays] DateType is not supported")
                };
            }

            return totalDays;
        }

        private int GetDaysInMonth(int monthsBack)
        {
            var targetDate = DateTime.Now.AddMonths(-monthsBack);
            return DateTime.DaysInMonth(targetDate.Year, targetDate.Month);
        }

        private int GetDaysInYear(int yearsBack)
        {
            var targetYear = DateTime.Now.Year - yearsBack;
            return DateTime.IsLeapYear(targetYear) ? 366 : 365;
        }
    }
}