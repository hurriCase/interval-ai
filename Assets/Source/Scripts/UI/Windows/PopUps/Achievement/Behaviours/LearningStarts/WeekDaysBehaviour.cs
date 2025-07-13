using System.Globalization;
using R3;
using Source.Scripts.Data.Repositories.User;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.PopUps.Achievement.Behaviours.LearningStarts
{
    internal sealed class WeekDaysBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] _weekDayTexts = new TextMeshProUGUI[7];

        internal void Init()
        {
            UserRepository.Instance.CultureChanged
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