using System;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Text;
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

        internal void UpdateLabels(int totalDays, int pointCount, DateTimeFormatInfo formatInfo)
        {
            if (TryGetDisplayRuleData(totalDays, out var displayType) is false)
                return;

            var dateRange = CalculateDatePointsForRange(totalDays, pointCount);
            PopulateDateLabels(displayType, dateRange, formatInfo);
        }

        private void PopulateDateLabels(
            DisplayType displayType,
            IReadOnlyList<DateTime> dateRange,
            DateTimeFormatInfo formatInfo)
        {
            for (var i = 0; i < _labels.Length; i++)
            {
                var label = _labels[i];
                var currentDate = dateRange[i];

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

        private bool TryGetDisplayRuleData(int dayCount, out DisplayType displayType)
        {
            displayType = DisplayType.DayOfWeek;
            foreach (var ruleData in _dateLabelConfig.DisplayRules)
            {
                if (ruleData.DayCount < dayCount)
                    continue;

                displayType = ruleData.DisplayType;
                return true;
            }

            return false;
        }

        private DateTime[] CalculateDatePointsForRange(int totalDays, int pointsCount)
        {
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-totalDays + 1);

            if (pointsCount == 1)
                return new[] { startDate };

            var datePoints = new DateTime[pointsCount];
            var intervalBetweenPoints = totalDays / (pointsCount - 1f);

            for (var i = 0; i < pointsCount; i++)
            {
                var daysFromStart = intervalBetweenPoints * i;
                datePoints[i] = startDate.AddDays(daysFromStart);
            }

            return datePoints;
        }
    }
}