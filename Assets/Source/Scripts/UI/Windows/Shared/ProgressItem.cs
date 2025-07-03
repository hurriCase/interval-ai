using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
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
        [field: SerializeField, Range(0f, 1f)] internal float AlphaForExtraDays { get; private set; }
        [field: SerializeField] internal int[] DefaultProgressPercentages { get; private set; } = new int[3];

        [field: SerializeField] internal List<ProgressSectionData> ProgressSections { get; private set; }

        private const int Circumference = 360;

        internal void Init(DailyProgress dailyProgress, string dateIdentifierText, bool shouldGrayOut = false)
        {
            DateIdentifierText.text = dateIdentifierText;

            if (shouldGrayOut)
            {
                SetDefault();
                FireIcon.SetActive(false);

                foreach (var sectionData in ProgressSections)
                {
                    var color = sectionData.RoundedFilledImage.color;
                    color.a = AlphaForExtraDays;
                    sectionData.RoundedFilledImage.color = color;
                }

                var color1 = DateIdentifierText.color;
                color1.a = AlphaForExtraDays;
                DateIdentifierText.color = color1;
                return;
            }

            var totalCount = dailyProgress.ProgressCountData.Sum();

            if (Validate(dailyProgress, totalCount) is false)
            {
                SetDefault();
                FireIcon.SetActive(false);
                return;
            }

            FireIcon.SetActive(true);

            //TODO:<Dmitriy.Sukharev> refactor
            DateIdentifierText.color = Color.HSVToRGB(17, 17, 17);

            SetProgress(dailyProgress, totalCount);
        }

        private void SetDefault()
        {
            DateIdentifierText.color = Color.gray;
            var offset = 0f;
            for (var i = 0; i < ProgressSections.Count; i++)
            {
                var sectionData = ProgressSections[i];
                if (DefaultProgressPercentages.Length <= i)
                {
                    sectionData.RoundedFilledImage.fillAmount = 0;
                    continue;
                }

                var totalPercentage = DefaultProgressPercentages.Sum();
                var progressRatio = (float)DefaultProgressPercentages[i] / totalPercentage;

                ProgressColorMapping.SetComponentForState(LearningState.None, sectionData.ImageTheme);

                sectionData.RoundedFilledImage.fillAmount = progressRatio - SpacingBetweenSections;
                sectionData.RoundedFilledImage.CustomFillOrigin = offset * Circumference;
                sectionData.RoundedFilledImage.ThicknessRatio = ThicknessRatio;

                offset += progressRatio;
            }
        }

        private void SetProgress(DailyProgress dailyProgress, int totalCount)
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

                ProgressColorMapping.SetComponentForState(sectionData.LearningState, sectionData.ImageTheme);

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