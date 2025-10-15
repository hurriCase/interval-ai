using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Words.Base;
using TMPro;
using UnityEngine;
using ZLinq;

namespace Source.Scripts.Main.UI.Shared.Progress
{
    internal class ProgressItem : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI progressLabel;
        [SerializeField] private ProgressColorMapping _progressColorMapping;
        [SerializeField] protected EnumArray<LearningState, ProgressSectionItem> progressSections = new(EnumMode.SkipFirst);
        [SerializeField] private GameObject _activeProgress;
        [SerializeField] private GameObject _inactiveProgress;

        [SerializeField] private float _spacingBetweenSections;
        [SerializeField] private float _activeThicknessRatio;
        [SerializeField] private float _inActiveThicknessRatio;

        private const int Circumference = 360;

        internal void Init(EnumArray<LearningState, int> progress, string labelText)
        {
            progressLabel.text = labelText;

            var totalCount = progress.Entries.AsValueEnumerable().Sum(static progress => progress.Value);
            var isActive = totalCount > 0;

            OnInit(isActive);

            _activeProgress.SetActive(isActive);
            _inactiveProgress.SetActive(isActive is false);

            if (isActive)
                SetProgress(progress, totalCount, _activeThicknessRatio);
        }

        protected virtual void OnInit(bool isActive) { }

        private void SetProgress(EnumArray<LearningState, int> progresses, int totalCount, float thicknessRatio)
        {
            var offset = 0f;
            var spacing = _spacingBetweenSections * thicknessRatio;

            var progressToDiscard = GetProgressToDiscard(progresses, totalCount, spacing);
            totalCount -= progressToDiscard;
            foreach (var (learningState, sectionData) in progressSections.AsTuples())
            {
                var wordCount = progresses[learningState];
                var progressRatio = (float)wordCount / totalCount;
                var fillAmount = progressRatio - spacing;
                if (wordCount <= 0 || fillAmount <= 0f)
                {
                    sectionData.RoundedFilledImage.fillAmount = 0;
                    continue;
                }

                _progressColorMapping.SetComponentForState(learningState, sectionData.ImageTheme);

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