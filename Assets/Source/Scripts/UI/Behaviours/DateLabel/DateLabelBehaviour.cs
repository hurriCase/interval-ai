using System;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Text;
using Source.Scripts.UI.Behaviours.DateLabel.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Behaviours.DateLabel
{
    internal sealed class DateLabelBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _labels;

        private IDateLabelConfig _dateLabelConfig;

        [Inject]
        internal void Inject(IDateLabelConfig dateLabelConfig)
        {
            _dateLabelConfig = dateLabelConfig;
        }

        internal void UpdateLabels(int totalDays, IReadOnlyList<DateTime> datePoints, DateTimeFormatInfo formatInfo)
        {
            if (TryGetDisplayRuleData(totalDays, out var displayRule) is false)
                return;

            PopulateDateLabels(datePoints, displayRule.DisplayType, formatInfo);
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

        private void PopulateDateLabels(
            IReadOnlyList<DateTime> datePoints,
            DisplayType displayType,
            DateTimeFormatInfo formatInfo)
        {
            var labelCount = _labels.Length;

            if (labelCount <= 0)
                return;

            for (var i = 0; i < labelCount; i++)
            {
                var label = _labels[i];
                var currentDate = datePoints[i];

                switch (displayType)
                {
                    case DisplayType.DayOfWeek:
                        label.text = formatInfo.GetAbbreviatedDayName(currentDate.DayOfWeek);
                        break;

                    case DisplayType.MonthWithDay:
                        var monthAbbreviation = formatInfo.GetAbbreviatedMonthName(currentDate.Month);
                        label.SetTextFormat("{0} {1}", currentDate.Day, monthAbbreviation);
                        break;

                    case DisplayType.Month:
                        label.text = formatInfo.GetAbbreviatedMonthName(currentDate.Month);
                        break;
                }
            }
        }
    }
}