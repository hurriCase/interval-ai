using System.Collections.Generic;
using R3;
using Source.Scripts.Data.Repositories.Settings;
using Source.Scripts.Data.Repositories.Settings.Base;
using Source.Scripts.Data.Repositories.User.Base;
using Source.Scripts.Data.Repositories.Words;
using Source.Scripts.Main.Source.Scripts.Main.UI.Shared;
using UnityEngine;
using VContainer;
using WordEntry = Source.Scripts.Data.Repositories.Words.Data.WordEntry;

namespace Source.Scripts.Main.Source.Scripts.Main.UI.PopUps.WordPractice.Behaviours.Cards
{
    internal sealed class WordProgressBehaviour : MonoBehaviour
    {
        [SerializeField] private ActivityMapping _activityMapping;
        [SerializeField] private ProgressSectionItem _progressSegment;
        [SerializeField] private float _spacingRatio;
        [SerializeField] private float _thicknessRatio;

        [Inject] private ISettingsRepository _settingsRepository;

        private const int Circumference = 360;

        private readonly List<ProgressSectionItem> _createdSegments = new();
        private int _previousSegmentCount;

        internal void Init()
        {
            _settingsRepository.RepetitionByCooldown
                .Subscribe(this, static (repetitions, behaviour)
                    => behaviour.CreateSegments(repetitions.Count))
                .RegisterTo(destroyCancellationToken);
        }

        //TODO:<Dmitriy.Sukharev> There are two problems with current solution.
        //First, we should use another visual for the segment based on design.
        //Second, the space isn't proportional between segments, probably due to rounded image internal logic.
        //And probably, fixing the first issue can resolve the second one too.
        private void CreateSegments(int segmentsCount)
        {
            if (_previousSegmentCount == segmentsCount)
                return;

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

        internal void UpdateProgress(WordEntry wordEntry)
        {
            for (var i = 0; i < _createdSegments.Count; i++)
            {
                var state = i < wordEntry.RepetitionCount ? ActivityState.Active : ActivityState.InActive;
                _activityMapping.SetComponentForState(state, _createdSegments[i].ImageTheme);
            }
        }
    }
}