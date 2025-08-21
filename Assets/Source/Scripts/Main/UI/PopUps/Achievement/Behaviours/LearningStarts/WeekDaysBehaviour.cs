using System.Globalization;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Settings.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts
{
    internal sealed class WeekDaysBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _weekDayTexts = new TextMeshProUGUI[DaysPerWeek];

        [Inject] private ISettingsRepository _settingsRepository;

        private const int DaysPerWeek = 7;

        internal void Init()
        {
            _settingsRepository.CurrentCulture
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