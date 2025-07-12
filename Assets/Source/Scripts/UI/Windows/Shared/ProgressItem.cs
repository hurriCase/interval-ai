using System.Linq;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.UI.Theme.Components;
using CustomUtils.Runtime.UI.Theme.ThemeMapping;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;
using TMPro;
using UnityEngine;

namespace Source.Scripts.UI.Windows.Shared
{
    internal sealed class ProgressItem : MonoBehaviour
    {
        [field: SerializeField] internal GameObject FireIcon { get; private set; }
        [field: SerializeField] internal TextMeshProUGUI DateIdentifierText { get; private set; }
        [field: SerializeField] internal TextThemeComponent DateIdentifierTheme { get; private set; }
        [field: SerializeField] internal float SpacingBetweenSections { get; private set; }
        [field: SerializeField] internal float ActiveThicknessRatio { get; private set; }
        [field: SerializeField] internal float InActiveThicknessRatio { get; private set; }
        [field: SerializeField, Range(0f, 1f)] internal float AlphaForExtraDays { get; private set; }
        [field: SerializeField] internal EnumArray<LearningState, int> DefaultProgressPercentages { get; private set; }
        [field: SerializeField] internal EnumArray<LearningState, ProgressSectionData> ProgressSections { get; private set; }

        private const int Circumference = 360;

        internal void Init(
            EnumArray<LearningState, int> progress,
            string dateIdentifierText,
            ThemeStateMappingGeneric<LearningState> progressColorMapping,
            ThemeStateMappingGeneric<DateIdentifierColorType> dateIdentifierMapping = null,
            bool isOutsideMonth = false)
        {
            DateIdentifierText.text = dateIdentifierText;

            var totalCount = progress.Values.Sum();

            if ((totalCount > 0) is false || isOutsideMonth)
            {
                SetProgress(DefaultProgressPercentages, progressColorMapping, InActiveThicknessRatio,
                    LearningState.None);
                if (FireIcon)
                    FireIcon.SetActive(false);

                if (dateIdentifierMapping)
                    dateIdentifierMapping.SetComponentForState(DateIdentifierColorType.InActive, DateIdentifierTheme);

                if (isOutsideMonth)
                    ApplyOutsideMonthEffect();
                return;
            }

            SetProgress(progress, progressColorMapping, ActiveThicknessRatio);
            if (FireIcon)
                FireIcon.SetActive(true);

            if (dateIdentifierMapping)
                dateIdentifierMapping.SetComponentForState(DateIdentifierColorType.Active, DateIdentifierTheme);
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
        private void SetProgress(EnumArray<LearningState, int> progresses,
            ThemeStateMappingGeneric<LearningState> progressColorMapping, float thicknessRatio,
            LearningState? overrideState = null)
        {
            var offset = 0f;
            var spacing = SpacingBetweenSections * thicknessRatio;
            var totalCount = progresses.Values.Sum();

            totalCount -= GetProgressToDiscard(progresses.Values, totalCount, spacing);
            foreach (var (state, sectionData) in ProgressSections.AsTuples())
            {
                var wordCount = progresses[state];
                var progressRatio = (float)wordCount / totalCount;
                var fillAmount = progressRatio - spacing;
                if (wordCount <= 0 || fillAmount <= 0f)
                {
                    sectionData.RoundedFilledImage.fillAmount = 0;
                    continue;
                }

                var learningState = overrideState ?? state;
                progressColorMapping.SetComponentForState(learningState, sectionData.ImageTheme);

                sectionData.RoundedFilledImage.fillAmount = fillAmount;
                sectionData.RoundedFilledImage.CustomFillOrigin = offset * Circumference;
                sectionData.RoundedFilledImage.ThicknessRatio = thicknessRatio;
                offset += progressRatio;
            }
        }

        private int GetProgressToDiscard(int[] progresses, int totalCount, float spacing)
        {
            var discardedProgresses = 0;
            foreach (var progress in progresses)
            {
                if ((float)progress / totalCount - spacing <= 0)
                    continue;

                discardedProgresses += progress;
            }

            return discardedProgresses;
        }
    }
}