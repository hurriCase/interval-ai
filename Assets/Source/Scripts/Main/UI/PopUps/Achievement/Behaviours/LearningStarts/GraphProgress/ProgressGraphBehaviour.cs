using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.UI.CustomComponents.Selectables.Toggles;
using R3;
using Source.Scripts.Core.Localization.Base;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.Data.Base;
using Source.Scripts.Main.UI.Shared.Progress;
using Source.Scripts.UI.Behaviours.DateLabel;
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
        [SerializeField] private StateToggle _graphTypeToggle;
        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] private EnumArray<LearningState, ThemeLineRenderer> _graphLines;

        [SerializeField] private DateLocalizationConfig _dateLocalizationConfig;

        private IProgressGraphSettings _progressGraphSettings;
        private IUISettingsRepository _uiSettingsRepository;
        private IGraphDataProcessor _graphDataProcessor;

        [Inject]
        internal void Inject(
            IProgressGraphSettings progressGraphSettings,
            IUISettingsRepository uiSettingsRepository,
            IGraphDataProcessor graphDataProcessor)
        {
            _progressGraphSettings = progressGraphSettings;
            _uiSettingsRepository = uiSettingsRepository;
            _graphDataProcessor = graphDataProcessor;
        }

        internal void Init()
        {
            foreach (var dateRange in _progressGraphSettings.GraphProgressRanges)
            {
                var createdGraphType = Instantiate(_graphTypeToggle, _graphButtonsContainer);
                createdGraphType.group = _graphButtonsGroup;

                createdGraphType.OnValueChangedAsObservable()
                    .Where(static isOn => isOn)
                    .SubscribeUntilDestroy(this, dateRange, static (range, self) => self.UpdateGraph(range));

                _dateLocalizationConfig.DateLocalizations[dateRange.DateType]
                    .SubscribePluralToText(dateRange.Amount, createdGraphType.Text);
            }
        }

        private void UpdateGraph(DateRange progressRange)
        {
            var totalDays = progressRange.CalculateDayCount(_uiSettingsRepository);
            var dateTimeFormatInfo = _uiSettingsRepository.CurrentCulture.Value.DateTimeFormat;
            var pointCount = _progressGraphSettings.GraphPointsCount;
            var displayData = _graphDataProcessor.GetDisplayGraphData(totalDays, pointCount);

            _dateLabelBehaviour.UpdateLabels(totalDays, pointCount, dateTimeFormatInfo);

            _maxProgressText.text = displayData.MaxProgress.ToString();

            RenderGraphLines(displayData);
        }

        private void RenderGraphLines(GraphDisplayData displayData)
        {
            foreach (var (learningState, themeLineRenderer) in _graphLines.AsTuples())
            {
                if (learningState == LearningState.Default)
                    return;

                _progressColorMapping.SetComponentForState(learningState, themeLineRenderer.ThemeComponent);

                var points = displayData.NormalizedPoints[learningState];
                themeLineRenderer.LineRenderer.SetPoints(points);
            }
        }
    }
}