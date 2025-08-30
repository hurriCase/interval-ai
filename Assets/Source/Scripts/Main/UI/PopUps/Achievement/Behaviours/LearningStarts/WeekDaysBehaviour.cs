using System.Globalization;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.Settings.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts
{
    internal sealed class WeekDaysBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _weekDayTexts = new TextMeshProUGUI[DaysPerWeek];

        private IUISettingsRepository _uiSettingsRepository;

        [Inject]
        internal void Inject(IUISettingsRepository uiSettingsRepository)
        {
            _uiSettingsRepository = uiSettingsRepository;
        }

        private const int DaysPerWeek = 7;

        internal void Init()
        {
            _uiSettingsRepository.CurrentCulture
                .SubscribeAndRegister(this, (culture, self) => self.UpdateWeekDays(culture));
        }

        private void UpdateWeekDays(CultureInfo culture)
        {
            var weekAbbreviatedNames = culture.DateTimeFormat.AbbreviatedDayNames;
            for (var i = 0; i < _weekDayTexts.Length; i++)
                _weekDayTexts[i].text = weekAbbreviatedNames[i];
        }
    }
}