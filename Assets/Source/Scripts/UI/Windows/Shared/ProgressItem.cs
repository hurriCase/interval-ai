using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.UI.Windows.Screens.LearningWords.Behaviours.Achievements;
using TMPro;
using UnityEngine;
using ZLinq;

namespace Source.Scripts.UI.Windows.Shared
{
    internal sealed class ProgressItem : MonoBehaviour
    {
        [field: SerializeField] internal ProgressColorMapping ProgressColorMapping { get; private set; }
        [field: SerializeField] internal GameObject FireIcon { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI DateIdentifierText { get; private set; }
        [field: SerializeField] internal float SpacingBetweenSections { get; private set; }
        [field: SerializeField] internal float ThicknessRatio { get; private set; }
        [field: SerializeField] internal List<ProgressSectionData> ProgressSections { get; private set; }

        private const int Circumference = 360;

        internal void Init(DailyProgress dailyProgress, string dateIdentifierText)
        {
            DateIdentifierText.text = dateIdentifierText;

            var totalCount = dailyProgress.ProgressCountData.Sum();

            if (Validate(dailyProgress, totalCount) is false)
            {
                FireIcon.SetActive(false);
                return;
            }

            FireIcon.SetActive(true);

            CalculateProgress(dailyProgress, totalCount);
        }

        private void CalculateProgress(DailyProgress dailyProgress, int totalCount)
        {
            var offset = 0f;
            foreach (var sectionData in ProgressSections)
            {
                var wordCount = dailyProgress.ProgressCountData[(int)sectionData.LearningState];
                if (wordCount <= 0)
                {
                    sectionData.RoundedFilledImage.fillAmount = 0;
                    continue;
                }

                ProgressColorMapping.SetComponentForState(sectionData.LearningState,
                    sectionData.ImageTheme);

                var progressRatio = (float)wordCount / totalCount;
                sectionData.RoundedFilledImage.fillAmount = progressRatio - SpacingBetweenSections;
                sectionData.RoundedFilledImage.CustomFillOrigin = offset * Circumference;
                sectionData.RoundedFilledImage.ThicknessRatio = ThicknessRatio;
                offset += progressRatio;
            }
        }

        private bool Validate(DailyProgress dailyProgress, int totalCount)
        {
            if (totalCount <= 0)
                return false;

            return dailyProgress.ProgressCountData.AsValueEnumerable()
                .Any(progressCount => progressCount > 0);
        }
    }
}