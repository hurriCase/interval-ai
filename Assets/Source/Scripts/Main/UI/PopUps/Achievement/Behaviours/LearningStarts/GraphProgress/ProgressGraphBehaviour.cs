using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.Data.Base;
using Source.Scripts.Main.UI.Shared.Progress;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Components.DateLabel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal sealed class ProgressGraphBehaviour : MonoBehaviour
    {
        [SerializeField] private DateLabelBehaviour _dateLabelBehaviour;

        [SerializeField] private TextMeshProUGUI _maxProgressText;

        [SerializeField] private RectTransform _graphButtonsContainer;
        [SerializeField] private ToggleGroup _graphButtonsGroup;
        [SerializeField] private ToggleComponent _graphTypeToggle;

        [SerializeField] private ProgressColorMapping _progressColorMapping;

        [SerializeField] private EnumArray<LearningState, ThemeLineRenderer> _graphLines = new(EnumMode.SkipFirst);

        private readonly Dictionary<LearningState, List<GraphProgressData>> _cashedAllProgressData = new();
        private readonly List<Vector2> _cashedNormalizedPoints = new();

        private ILocalizationKeysDatabase _localizationKeysDatabase;
        private IProgressGraphSettings _progressGraphSettings;
        private IDateProgressService _dateProgressService;

        [Inject]
        internal void Inject(
            ILocalizationKeysDatabase localizationKeysDatabase,
            IProgressGraphSettings progressGraphSettings,
            IDateProgressService dateProgressService)
        {
            _localizationKeysDatabase = localizationKeysDatabase;
            _progressGraphSettings = progressGraphSettings;
            _dateProgressService = dateProgressService;
        }

        internal void Init()
        {
            _dateLabelBehaviour.Init();

            foreach (var dateRange in _progressGraphSettings.GraphProgressRanges)
            {
                var createdGraphType = Instantiate(_graphTypeToggle, _graphButtonsContainer);
                createdGraphType.group = _graphButtonsGroup;
                createdGraphType.OnValueChangedAsObservable()
                    .Where(isOn => isOn)
                    .SubscribeUntilDestroy(this, dateRange, static (dateRange, self) => self.UpdateGraph(dateRange));

                LocalizationController.Language.SubscribeUntilDestroy(this, (dateRange, createdGraphType.Text),
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
            _dateLabelBehaviour.CurrentDateType.Value = progressRange;

            var maxProgress = GenerateAllGraphPoints(progressRange);
            _maxProgressText.text = maxProgress.ToString();

            foreach (var (learningState, themeLineRenderer) in _graphLines.AsTuples())
            {
                _progressColorMapping.SetComponentForState(learningState, themeLineRenderer.ThemeComponent);

                var normalizedPoints = NormalizePoints(
                    _cashedAllProgressData[learningState],
                    maxProgress,
                    _progressGraphSettings.GraphPointsCount);

                themeLineRenderer.LineRenderer.SetPoints(normalizedPoints);
            }
        }

        private int GenerateAllGraphPoints(DateRange progressRange)
        {
            var totalDays = progressRange.GetDayCount();
            var pointsCount = _progressGraphSettings.GraphPointsCount;
            var daysPerSegment = (float)totalDays / pointsCount;
            var maxProgress = 0;

            foreach (var (learningState, _) in _graphLines.AsTuples())
            {
                if (_cashedAllProgressData
                        .TryGetValue(learningState, out var progressPoints) is false)
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

            if (maxProgress <= 0)
                return _cashedNormalizedPoints;

            foreach (var (index, progress) in points)
            {
                var normalizedX = (float)index / (maxIndex - 1);
                var normalizedY = (float)progress / maxProgress;

                _cashedNormalizedPoints.Add(new Vector2(normalizedX, normalizedY));
            }

            return _cashedNormalizedPoints;
        }
    }
}