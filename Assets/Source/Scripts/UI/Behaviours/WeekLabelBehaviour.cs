using System.Globalization;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.Settings.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.UI.Behaviours
{
    internal sealed class WeekLabelBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _weekDayTexts = new TextMeshProUGUI[DaysPerWeek];

        private const int DaysPerWeek = 7;

        private IUISettingsRepository _uiSettingsRepository;

        [Inject]
        internal void Inject(IUISettingsRepository uiSettingsRepository)
        {
            _uiSettingsRepository = uiSettingsRepository;
        }

        internal void Init()
        {
            _uiSettingsRepository.CurrentCulture
                .SubscribeUntilDestroy(this, (culture, self) => self.UpdateWeekDays(culture));
        }

        private void UpdateWeekDays(CultureInfo culture)
        {
            var weekAbbreviatedNames = culture.DateTimeFormat.AbbreviatedDayNames;
            for (var i = 0; i < _weekDayTexts.Length; i++)
                _weekDayTexts[i].text = weekAbbreviatedNames[i];
        }
    }
}