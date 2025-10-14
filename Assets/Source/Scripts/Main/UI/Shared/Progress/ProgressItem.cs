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
    internal sealed class ProgressItem : MonoBehaviour
    {
        [SerializeField] private GameObject _fireIcon;
        [SerializeField] private TextMeshProUGUI _dateIdentifierText;
        [SerializeField] private ThemeComponent _dateIdentifierTheme;
        [SerializeField] private float _spacingBetweenSections;
        [SerializeField] private float _activeThicknessRatio;
        [SerializeField] private float _inActiveThicknessRatio;
        [SerializeField] [field: Range(0f, 1f)] private float _alphaForExtraDays;
        [SerializeField] private EnumArray<LearningState, int> _defaultProgressPercentages = new(EnumMode.SkipFirst);

        [SerializeField] private EnumArray<LearningState, ProgressSectionItem> _progressSections = new(EnumMode.SkipFirst);

        private const int Circumference = 360;

        internal void Init(
            EnumArray<LearningState, int> progress,
            string dateIdentifierText,
            ThemeStateMappingGeneric<LearningState> progressColorMapping,
            ThemeStateMappingGeneric<ActivityState> dateIdentifierMapping = null,
            bool isOutsideMonth = false)
        {
            _dateIdentifierText.text = dateIdentifierText;

            var totalCount = progress.Entries.AsValueEnumerable().Sum(entry => entry.Value);
            var isActive = totalCount > 0 && isOutsideMonth is false;

            if (isActive)
                SetProgress(progress, totalCount, progressColorMapping, _activeThicknessRatio);
            else
                SetProgress(_defaultProgressPercentages,
                    _defaultProgressPercentages.Entries.AsValueEnumerable().Sum(entry => entry.Value),
                    progressColorMapping,
                    _inActiveThicknessRatio,
                    LearningState.Default);

            if (_fireIcon)
                _fireIcon.SetActive(isActive);

            if (isOutsideMonth)
                ApplyOutsideMonthEffect();

            if (!dateIdentifierMapping)
                return;

            var dateIdentifierColorType = isActive ? ActivityState.Active : ActivityState.InActive;
            dateIdentifierMapping.SetComponentForState(dateIdentifierColorType, _dateIdentifierTheme);
        }

        private void ApplyOutsideMonthEffect()
        {
            foreach (var sectionData in _progressSections)
                sectionData.RoundedFilledImage.SetAlpha(_alphaForExtraDays);

            _dateIdentifierText.SetAlpha(_alphaForExtraDays);
        }

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
            foreach (var (state, sectionData) in _progressSections.AsTuples())
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