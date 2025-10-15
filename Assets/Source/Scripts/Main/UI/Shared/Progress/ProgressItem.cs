using CustomUtils.Runtime.Constants;
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

        [SerializeField] private float _spaceRatio;

        internal void Init(EnumArray<LearningState, int> progress, string labelText)
        {
            progressLabel.text = labelText;

            var totalCount = progress.Entries.AsValueEnumerable().Sum(static progress => progress.Value);
            var isActive = totalCount > 0;

            OnInit(isActive);

            _activeProgress.SetActive(isActive);
            _inactiveProgress.SetActive(isActive is false);

            if (isActive)
                SetProgress(progress, totalCount);
        }

        protected virtual void OnInit(bool isActive) { }

        private void SetProgress(EnumArray<LearningState, int> progresses, int totalProgress)
        {
            var offset = 0f;

            var validProgressCount = progresses.Entries.AsValueEnumerable()
                .Count(static progress => progress.Value > 0);

            var spaceRatio = validProgressCount == 1 ? 0 : _spaceRatio;
            var totalSpaceRatio = spaceRatio * validProgressCount;
            var totalProgressWithSpace = totalProgress / (1 - totalSpaceRatio);

            foreach (var (learningState, sectionData) in progressSections.AsTuples())
            {
                var progress = progresses[learningState];
                if (progress <= 0)
                {
                    sectionData.RoundedFilledImage.fillAmount = 0;
                    continue;
                }

                var progressRatio = progress / totalProgressWithSpace;
                sectionData.RoundedFilledImage.fillAmount = progressRatio;

                offset += spaceRatio;
                sectionData.RoundedFilledImage.CustomFillOrigin = offset * MathConstants.FullCircleDegrees;
                offset += progressRatio;

                _progressColorMapping.SetComponentForState(learningState, sectionData.ImageTheme);
            }
        }
    }
}