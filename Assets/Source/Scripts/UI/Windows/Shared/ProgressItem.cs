using System.Collections.Generic;
using System.Linq;
using CustomUtils.Runtime.UI.Theme.Components;
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
        [field: SerializeField] internal DateIdentifierMapping DateIdentifierMapping { get; private set; }
        [field: SerializeField] internal GameObject FireIcon { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI DateIdentifierText { get; private set; }
        [field: SerializeField] internal TextThemeComponent DateIdentifierTheme { get; private set; }
        [field: SerializeField] internal float SpacingBetweenSections { get; private set; }
        [field: SerializeField] internal float ActiveThicknessRatio { get; private set; }
        [field: SerializeField] internal float InActiveThicknessRatio { get; private set; }
        [field: SerializeField, Range(0f, 1f)] internal float AlphaForExtraDays { get; private set; }
        [field: SerializeField] internal int[] DefaultProgressPercentages { get; private set; } = new int[5];
        [field: SerializeField] internal List<ProgressSectionData> ProgressSections { get; private set; }

        private const int Circumference = 360;

        internal void Init(DailyProgress dailyProgress, string dateIdentifierText, bool isOutsideMonth = false)
        {
            DateIdentifierText.text = dateIdentifierText;

            var totalCount = dailyProgress.ProgressCountData.Sum();

            if ((totalCount > 0 && dailyProgress.ProgressCountData.AsValueEnumerable()
                    .Any(progressCount => progressCount > 0) is false) || isOutsideMonth)
            {
                SetProgress(DefaultProgressPercentages, InActiveThicknessRatio, LearningState.None);
                FireIcon.SetActive(false);
                DateIdentifierMapping.SetComponentForState(DateIdentifierColorType.InActive, DateIdentifierTheme);

                if (isOutsideMonth)
                    ApplyOutsideMonthEffect();
                return;
            }

            SetProgress(dailyProgress.ProgressCountData, ActiveThicknessRatio);
            FireIcon.SetActive(true);
            DateIdentifierMapping.SetComponentForState(DateIdentifierColorType.Active, DateIdentifierTheme);
        }

        private void ApplyOutsideMonthEffect()
        {
            foreach (var sectionData in ProgressSections)
            {
                var color = sectionData.RoundedFilledImage.color;
                color.a = AlphaForExtraDays;
                sectionData.RoundedFilledImage.color = color;
            }

            var color1 = DateIdentifierText.color;
            color1.a = AlphaForExtraDays;
            DateIdentifierText.color = color1;
        }

        // TODO: <Dmitriy.Sukharev> Fix invisible micro-progress - show minimum visible progress instead of discarding
        private void SetProgress(IReadOnlyList<int> progresses, float thicknessRatio, LearningState? overrideState = null)
        {
            var offset = 0f;
            var spacing = SpacingBetweenSections * thicknessRatio;
            var totalCount = progresses.Sum();
            var discardedProgresses = progresses.Where(progress => (float)progress / totalCount - spacing <= 0).Sum();
            totalCount -= discardedProgresses;
            foreach (var sectionData in ProgressSections)
            {
                var wordCount = progresses[(int)sectionData.LearningState];
                var progressRatio = (float)wordCount / totalCount;
                var fillAmount = progressRatio - spacing;
                if (wordCount <= 0 || fillAmount <= 0f)
                {
                    sectionData.RoundedFilledImage.fillAmount = 0;
                    Debug.LogWarning($"[ProgressItem::SetProgress] skipped", gameObject);
                    continue;
                }

                var learningState = overrideState ?? sectionData.LearningState;
                ProgressColorMapping.SetComponentForState(learningState, sectionData.ImageTheme);

                sectionData.RoundedFilledImage.fillAmount = fillAmount;
                sectionData.RoundedFilledImage.CustomFillOrigin = offset * Circumference;
                sectionData.RoundedFilledImage.ThicknessRatio = thicknessRatio;
                offset += progressRatio;
            }
        }
    }
}