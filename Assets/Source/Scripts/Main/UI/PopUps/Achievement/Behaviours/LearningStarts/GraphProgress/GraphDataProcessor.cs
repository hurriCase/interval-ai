using System;
using System.Collections.Generic;
using System.Linq;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal sealed class GraphDataProcessor : IGraphDataProcessor
    {
        private const int GraphRangeExpansion = 1;

        private readonly IDateProgressService _dateProgressService;

        internal GraphDataProcessor(IDateProgressService dateProgressService)
        {
            _dateProgressService = dateProgressService;
        }

        public GraphDisplayData GetDisplayGraphData(int totalDays, int pointsCount)
        {
            var rawGraphData = GetGraphDataForRange(totalDays, pointsCount);
            var maxProgress = CalculateMaxProgress(rawGraphData);

            if (maxProgress == 0)
                return GraphDisplayData.Empty;

            var normalizedData = NormalizeAllData(rawGraphData, maxProgress);

            return new GraphDisplayData(maxProgress, normalizedData);
        }

        private EnumArray<LearningState, int[]> GetGraphDataForRange(int totalDays, int pointsCount)
        {
            var graphData = new EnumArray<LearningState, int[]>(EnumMode.SkipFirst);
            var daysPerSegment = (float)totalDays / pointsCount;

            foreach (var (state, _) in graphData.AsTuples())
            {
                var progressData = new int[pointsCount];

                for (var i = 0; i < pointsCount; i++)
                {
                    var (daysBack, duration) = CalculateSegmentRange(i, pointsCount, daysPerSegment);
                    progressData[i] = _dateProgressService.GetProgressForRange(daysBack, duration, state);
                }

                graphData[state] = progressData;
            }

            return graphData;
        }

        private int CalculateMaxProgress(EnumArray<LearningState, int[]> graphData)
        {
            var maxProgress = 0;

            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator | Due to boxing
            foreach (var progressData in graphData)
                maxProgress = Math.Max(maxProgress, progressData.Max());

            return maxProgress;
        }

        private EnumArray<LearningState, List<Vector2>> NormalizeAllData(
            EnumArray<LearningState, int[]> rawData,
            int maxProgress)
        {
            var result = new EnumArray<LearningState, List<Vector2>>(EnumMode.SkipFirst);

            foreach (var (state, progressData) in rawData.AsTuples())
            {
                var displayRange = GetDisplayRange(progressData);

                if (displayRange.HasValue is false)
                {
                    result[state] = new List<Vector2>();
                    continue;
                }

                var (start, end) = displayRange.Value;
                result[state] = ConvertToNormalizedPoints(progressData, start, end, maxProgress);
            }

            return result;
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