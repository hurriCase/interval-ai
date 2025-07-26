using System.Globalization;
using R3;
using Source.Scripts.Data.Repositories.Settings;
using Source.Scripts.Data.Repositories.Settings.Base;
using TMPro;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts
{
    internal sealed class WeekDaysBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _weekDayTexts = new TextMeshProUGUI[7];

        [Inject] private ISettingsRepository _settingsRepository;

        internal void Init()
        {
            _settingsRepository.CurrentCulture
                .Subscribe(this, (culture, behaviour) => behaviour.UpdateWeekDays(culture))
                .RegisterTo(destroyCancellationToken);
        }

        private void UpdateWeekDays(CultureInfo culture)
        {
            var weekAbbreviatedNames = culture.DateTimeFormat.AbbreviatedDayNames;
            for (var i = 0; i < _weekDayTexts.Length; i++)
                _weekDayTexts[i].text = weekAbbreviatedNames[i];
        }
    }
}