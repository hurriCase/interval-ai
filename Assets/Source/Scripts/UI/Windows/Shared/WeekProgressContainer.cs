using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Data.Repositories.Progress.Entries;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Shared
{
    internal sealed class WeekProgressContainer : MonoBehaviour
    {
        [SerializeField] private List<ProgressItem> _progressItems;

        internal void UpdateWeeklyProgress(List<DailyProgress> weeklyProgress, List<string> weekDayIdentifierTexts)
        {
            if (_progressItems.Count < weeklyProgress.Count || weekDayIdentifierTexts.Count < weeklyProgress.Count)
            {
                Debug.LogError("[AchievementsBehaviour::Init] There is a mismatch between element counts:\n" +
                               $"_progressItems.Count: {_progressItems.Count}\n" +
                               $"currentWeek.Count: {weeklyProgress.Count}\n" +
                               $"localizationKeys.Count: {weekDayIdentifierTexts.Count}");
                return;
            }

            for (var i = 0; i < _progressItems.Count; i++)
            {
                if ((int)weeklyProgress.First().DateTime.DayOfWeek <= i ||
                    (int)weeklyProgress.Last().DateTime.DayOfWeek >= i)
                    _progressItems[i].Init(weeklyProgress[i], weekDayIdentifierTexts[i]);
                else
                    _progressItems[i].gameObject.SetActive(false);
            }
        }
    }
}