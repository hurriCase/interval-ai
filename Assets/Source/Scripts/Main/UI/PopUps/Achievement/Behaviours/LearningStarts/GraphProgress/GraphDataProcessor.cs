using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal sealed class GraphDataProcessor : IGraphDataProcessor
    {
        public int MaxProgress { get; private set; }
        public EnumArray<LearningState, List<Vector2>> NormalizedPoints { get; } = new(new List<Vector2>());

        private const int GraphRangeExpansion = 1;

        private readonly IDateProgressService _dateProgressService;

        private readonly EnumArray<LearningState, int[]> _graphData = new();

        internal GraphDataProcessor(IDateProgressService dateProgressService)
        {
            _dateProgressService = dateProgressService;
        }

        public void GetDisplayGraphData(int totalDays, int pointsCount)
        {
            UpdateGraphDataForRange(totalDays, pointsCount);
            UpdateMaxProgress();

            if (MaxProgress == 0)
            {
                NormalizedPoints.Clear();
                return;
            }

            UpdateNormalizeAllData();
        }

        private void UpdateGraphDataForRange(int totalDays, int pointsCount)
        {
            var daysPerSegment = (float)totalDays / pointsCount;

            foreach (var (state, _) in _graphData.AsTuples())
            {
                var progressData = new int[pointsCount];

                for (var i = 0; i < pointsCount; i++)
                {
                    var (daysBack, duration) = CalculateSegmentRange(i, pointsCount, daysPerSegment);
                    progressData[i] = _dateProgressService.GetProgressForRange(daysBack, duration, state);
                }

                _graphData[state] = progressData;
            }
        }

        private void UpdateMaxProgress()
        {
            MaxProgress = 0;

            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator | To prevent boxing
            foreach (var progressData in _graphData)
                MaxProgress = Math.Max(MaxProgress, progressData.Max());
        }

        private void UpdateNormalizeAllData()
        {
            foreach (var (state, progressData) in _graphData.AsTuples())
            {
                var displayRange = GetDisplayRange(progressData);
                if (displayRange.HasValue is false)
                    continue;

                var (start, end) = displayRange.Value;
                NormalizedPoints[state] = ConvertToNormalizedPoints(progressData, start, end, MaxProgress);
            }
        }

        private (int daysBack, int duration) CalculateSegmentRange(
            int pointIndex,
            int totalPoints,
            float daysPerSegment)
        {
            var segmentFromEnd = totalPoints - 1 - pointIndex;

            var daysBack = (int)(daysPerSegment * segmentFromEnd);
            var segmentEnd = (int)(daysPerSegment * (segmentFromEnd + 1));
            var duration = Math.Max(1, segmentEnd - daysBack);

            return (daysBack, duration);
        }

        private (int start, int end)? GetDisplayRange(IReadOnlyList<int> progressData)
        {
            var nonZeroRange = progressData.GetNonZeroRange();

            if (nonZeroRange.HasValue is false)
                return null;

            var (start, end) = nonZeroRange.Value;

            var expandedStart = Math.Max(0, start - GraphRangeExpansion);
            var expandedEnd = Math.Min(progressData.Count - 1, end + GraphRangeExpansion);
            return (expandedStart, expandedEnd);
        }

        private List<Vector2> ConvertToNormalizedPoints(
            IReadOnlyList<int> progressData,
            int startIndex,
            int endIndex,
            int maxProgress)
        {
            var points = new List<Vector2>(endIndex - startIndex + 1);
            var maxIndex = progressData.Count - 1;

            for (var i = startIndex; i <= endIndex; i++)
            {
                var normalizedX = (float)i / maxIndex;
                var normalizedY = (float)progressData[i] / maxProgress;
                points.Add(new Vector2(normalizedX, normalizedY));
            }

            return points;
        }
    }
}