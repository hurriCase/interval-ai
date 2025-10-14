using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.UI.Theme;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.Shared.Activity;
using TMPro;
using UnityEngine;
using ZLinq;

namespace Source.Scripts.Main.UI.Shared.Progress
{
    internal class ProgressItem : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI dateIdentifierText;
        [SerializeField] protected EnumArray<LearningState, ProgressSectionItem> progressSections = new(EnumMode.SkipFirst);

        [SerializeField] private ThemeComponent _dateIdentifierTheme;
        [SerializeField] private float _spacingBetweenSections;
        [SerializeField] private float _activeThicknessRatio;
        [SerializeField] private float _inActiveThicknessRatio;
        [SerializeField] private EnumArray<LearningState, int> _defaultProgressPercentages = new(EnumMode.SkipFirst);

        private const int Circumference = 360;

        internal void Init(
            EnumArray<LearningState, int> progress,
            string dateIdentifierText,
            ThemeStateMappingGeneric<LearningState> progressColorMapping,
            ThemeStateMappingGeneric<ActivityState> dateIdentifierMapping = null)
        {
            this.dateIdentifierText.text = dateIdentifierText;

            var totalCount = progress.Entries.AsValueEnumerable().Sum(entry => entry.Value);
            var isActive = totalCount > 0;

            if (isActive)
                SetProgress(progress, totalCount, progressColorMapping, _activeThicknessRatio);
            else
                SetProgress(_defaultProgressPercentages,
                    _defaultProgressPercentages.Entries.AsValueEnumerable().Sum(entry => entry.Value),
                    progressColorMapping,
                    _inActiveThicknessRatio,
                    LearningState.Default);

            OnInit(isActive);

            if (!dateIdentifierMapping)
                return;

            var dateIdentifierColorType = isActive ? ActivityState.Active : ActivityState.InActive;
            dateIdentifierMapping.SetComponentForState(dateIdentifierColorType, _dateIdentifierTheme);
        }

        protected virtual void OnInit(bool isActive) { }

        // TODO: <Dmitriy.Sukharev> Fix invisible micro-progress - show minimum visible progress instead of discarding
        private void SetProgress(
            EnumArray<LearningState, int> progresses,
            int totalCount,
            ThemeStateMappingGeneric<LearningState> progressColorMapping,
            float thicknessRatio,
            LearningState? overrideState = null)
        {
            var offset = 0f;
            var spacing = _spacingBetweenSections * thicknessRatio;

            var progressToDiscard = GetProgressToDiscard(progresses, totalCount, spacing);
            totalCount -= progressToDiscard;
            foreach (var (state, sectionData) in progressSections.AsTuples())
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

        private int GetProgressToDiscard(EnumArray<LearningState, int> progresses, int totalCount, float spacing)
        {
            var discardedProgresses = 0;
            foreach (var progress in progresses)
            {
                if ((float)progress / totalCount - spacing <= 0)
                    discardedProgresses += progress;
            }

            return discardedProgresses;
        }
    }
}