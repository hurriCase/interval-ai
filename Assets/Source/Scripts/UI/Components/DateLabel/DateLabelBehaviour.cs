using System;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using Cysharp.Text;
using R3;
using Source.Scripts.Core.Repositories.Settings.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Components.DateLabel
{
    internal sealed class DateLabelBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _labels;

        internal ReactiveProperty<DateRange> CurrentDateType { get; } = new();

        private IUISettingsRepository _uiSettingsRepository;
        private IDateLabelConfig _dateLabelConfig;

        [Inject]
        internal void Inject(IUISettingsRepository uiSettingsRepository, IDateLabelConfig dateLabelConfig)
        {
            _uiSettingsRepository = uiSettingsRepository;
            _dateLabelConfig = dateLabelConfig;
        }

        internal void Init()
        {
            _uiSettingsRepository.CurrentCulture.SubscribeAndRegister(this, static self => self.UpdateLabels());
            CurrentDateType.SubscribeUntilDestroy(this, static self => self.UpdateLabels());
        }

        private void UpdateLabels()
        {
            var dayCount = CurrentDateType.Value.GetDayCount();

            if (TryGetDisplayRuleData(dayCount, out var displayRule) is false)
                return;

            PopulateDateLabels(dayCount, displayRule.DisplayType);
        }

        private bool TryGetDisplayRuleData(int dayCount, out DisplayRuleData displayRuleData)
        {
            displayRuleData = null;
            foreach (var ruleData in _dateLabelConfig.DisplayRules)
            {
                if (ruleData.DayCount < dayCount)
                    continue;

                displayRuleData = ruleData;
                return true;
            }

            return false;
        }

        private void PopulateDateLabels(int dayCount, DisplayType displayType)
        {
            var dateTimeFormat = _uiSettingsRepository.CurrentCulture.Value.DateTimeFormat;
            var labelCount = _labels.Length;

            if (labelCount <= 0)
                return;

            var now = DateTime.Now.Date;
            var startDate = now.AddDays(-dayCount);
            var totalDays = (now - startDate).Days;

            for (var i = 0; i < labelCount; i++)
            {
                var label = _labels[i];
                var progress = labelCount > 1 ? (float)i / (labelCount - 1) : 0;
                var currentDate = startDate.AddDays(totalDays * progress);

                switch (displayType)
                {
                    case DisplayType.DayOfWeek:
                        label.text = dateTimeFormat.GetAbbreviatedDayName(currentDate.DayOfWeek);
                        break;

                    case DisplayType.MonthWithDay:
                        var monthAbbreviation = dateTimeFormat.GetAbbreviatedMonthName(currentDate.Month);
                        label.SetTextFormat("{0} {1}", currentDate.Day, monthAbbreviation);
                        break;

                    case DisplayType.Month:
                        label.text = dateTimeFormat.GetAbbreviatedMonthName(currentDate.Month);
                        break;
                }
            }
        }
    }
}