using CustomUtils.Runtime.Constants;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Others.UIPools;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Shared.Activity;
using Source.Scripts.Main.UI.Shared.Progress;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class WordProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private ProgressSectionItem _progressSegment;
        [SerializeField] private RectTransform _segmentContainer;
        [SerializeField] private ActivityMapping _activityMapping;

        [SerializeField] private float _spacingRatio;

        private UIPool<ProgressSectionItem> _progressPool;

        private IPracticeSettingsRepository _practiceSettingsRepository;

        [Inject]
        internal void Inject(IPracticeSettingsRepository practiceSettingsRepository)
        {
            _practiceSettingsRepository = practiceSettingsRepository;
        }

        internal void Init()
        {
            _progressPool = new UIPool<ProgressSectionItem>(_progressSegment, _segmentContainer);

            _practiceSettingsRepository.RepetitionByCooldown.SubscribeUntilDestroy(this,
                static (repetitions, self) => self.CreateSegments(repetitions.Count));
        }

        private void CreateSegments(int segmentsCount)
        {
            _progressPool.EnsureCount(segmentsCount);

            var segmentFill = 1f / segmentsCount;
            var actualSpacing = segmentFill * _spacingRatio;

            var offset = 0f;
            foreach (var sectionItem in _progressPool.ActiveItems)
            {
                sectionItem.RoundedFilledImage.CustomFillOrigin = offset * MathConstants.FullCircleDegrees;
                sectionItem.RoundedFilledImage.fillAmount = segmentFill - actualSpacing;
                offset += segmentFill;
            }
        }

        public void UpdateProgress(WordEntry wordEntry)
        {
            for (var i = 0; i < _progressPool.ActiveItems.Count; i++)
            {
                var state = i < wordEntry.ReviewCount ? ActivityState.Active : ActivityState.InActive;
                _activityMapping.SetComponentForState(state, _progressPool.ActiveItems[i].ImageTheme);
            }
        }
    }
}