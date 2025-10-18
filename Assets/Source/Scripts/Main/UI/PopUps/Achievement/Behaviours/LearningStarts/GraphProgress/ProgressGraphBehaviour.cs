using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions.Observables;
using CustomUtils.Runtime.Localization;
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
        [SerializeField] private EnumArray<LearningState, ThemeLineRenderer> _graphLines = new(EnumMode.SkipFirst);

        private ILocalizationKeysDatabase _localizationKeysDatabase;
        private IProgressGraphSettings _progressGraphSettings;
        private IUISettingsRepository _uiSettingsRepository;
        private IGraphDataProcessor _graphDataProcessor;

        [Inject]
        internal void Inject(
            ILocalizationKeysDatabase localizationKeysDatabase,
            IProgressGraphSettings progressGraphSettings,
            IUISettingsRepository uiSettingsRepository,
            IGraphDataProcessor graphDataProcessor)
        {
            _localizationKeysDatabase = localizationKeysDatabase;
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
                    .Where(isOn => isOn)
                    .SubscribeUntilDestroy(this, dateRange, static (range, self) => self.UpdateGraph(range));

                LocalizationController.Language.SubscribeUntilDestroy(this, (dateRange, createdGraphType.Text),
                    static (tuple, self) => self.UpdateLocalization(tuple.dateRange, tuple.Text));
            }
        }

        private void UpdateLocalization(DateRange dateRange, TMP_Text graphTypeText)
        {
            var localizationKey = _localizationKeysDatabase.GetDateLocalization(
                dateRange.DateType,
                dateRange.Amount);

            graphTypeText.SetText(localizationKey, dateRange.Amount);
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
                _progressColorMapping.SetComponentForState(learningState, themeLineRenderer.ThemeComponent);

                var points = displayData.NormalizedPoints[learningState];
                themeLineRenderer.LineRenderer.SetPoints(points);
            }
        }
    }
}