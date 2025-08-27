using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.Settings.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Shared;
using UnityEngine;
using VContainer;

namespace Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours
{
    internal sealed class WordProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private ActivityMapping _activityMapping;
        [SerializeField] private ProgressSectionItem _progressSegment;
        [SerializeField] private float _spacingRatio;
        [SerializeField] private float _thicknessRatio;

        [Inject] private IPracticeSettingsRepository _practiceSettingsRepository;
        [Inject] private ICurrentWordsService _currentWordsService;

        private const int Circumference = 360;

        private readonly List<ProgressSectionItem> _createdSegments = new();
        private int _previousSegmentCount;

        internal void Init()
        {
            _practiceSettingsRepository.RepetitionByCooldown.SubscribeAndRegister(this,
                static (repetitions, self) => self.CreateSegments(repetitions.Count));
        }

        private void CreateSegments(int segmentsCount)
        {
            if (_previousSegmentCount == segmentsCount)
                return;

            foreach (var segment in _createdSegments)
                Destroy(segment.gameObject);

            _createdSegments.Clear();

            var segmentFill = 1f / segmentsCount;
            var actualSpacing = segmentFill * _spacingRatio;

            var offset = 0f;
            for (var i = 0; i < segmentsCount; i++)
            {
                var createdSegment = Instantiate(_progressSegment, transform);

                createdSegment.RoundedFilledImage.CustomFillOrigin = offset * Circumference;
                createdSegment.RoundedFilledImage.ThicknessRatio = _thicknessRatio;
                createdSegment.RoundedFilledImage.fillAmount = segmentFill - actualSpacing;
                offset += segmentFill;

                _createdSegments.Add(createdSegment);
            }

            _previousSegmentCount = segmentsCount;
        }

        public void UpdateProgress(WordEntry wordEntry)
        {
            for (var i = 0; i < _createdSegments.Count; i++)
            {
                var state = i < wordEntry.ReviewCount ? ActivityState.Active : ActivityState.InActive;
                _activityMapping.SetComponentForState(state, _createdSegments[i].ImageTheme);
            }
        }
    }
}