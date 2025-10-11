using System;
using System.Collections.Generic;
using System.Linq;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal sealed class GraphDataProcessor : IGraphDataProcessor
    {
        private readonly IDateProgressService _dateProgressService;

        internal GraphDataProcessor(IDateProgressService dateProgressService)
        {
            _dateProgressService = dateProgressService;
        }

        public GraphDisplayData GetDisplayGraphData(int totalDays, int pointsCount)
        {
            var rangeData = Calculate(totalDays, pointsCount);
            var rawGraphData = GetGraphDataForRange(totalDays, pointsCount);
            var maxProgress = CalculateMaxProgress(rawGraphData);

            if (maxProgress == 0)
                return GraphDisplayData.Empty;

            var normalizedData = NormalizeAllData(rawGraphData, maxProgress);

            return new GraphDisplayData(maxProgress, normalizedData, rangeData);
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
                    var segmentIndex = pointsCount - 1 - i;
                    var daysBack = (int)(daysPerSegment * segmentIndex);
                    var duration = Math.Max(1, (int)(daysPerSegment * (segmentIndex + 1)) - daysBack);

                    progressData[i] = _dateProgressService.GetProgressForRange(daysBack, duration, state);
                }

                graphData[state] = progressData;
            }

            return graphData;
        }

        private DateTime[] Calculate(int totalDays, int pointsCount)
        {
            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-totalDays + 1);
            var datePoints = CalculateDatePoints(startDate, totalDays, pointsCount);

            return datePoints;
        }

        private DateTime[] CalculateDatePoints(DateTime startDate, int totalDays, int pointsCount)
        {
            if (pointsCount == 1)
                return new[] { startDate };

            var datePoints = new DateTime[pointsCount];
            var intervalBetweenPoints = totalDays / (pointsCount - 1f);

            for (var i = 0; i < pointsCount; i++)
            {
                var daysFromStart = intervalBetweenPoints * i;
                datePoints[i] = startDate.AddDays(daysFromStart);
            }

            return datePoints;
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
                var trimmedRange = GetNonZeroRange(progressData);

                if (trimmedRange.HasValue is false)
                {
                    result[state] = new List<Vector2>();
                    continue;
                }

                var (startIndex, endIndex) = trimmedRange.Value;

                var expandedStart = Math.Max(0, startIndex - 1);
                var expandedEnd = Math.Min(progressData.Length - 1, endIndex + 1);

                var points = new List<Vector2>(expandedEnd - expandedStart + 1);

                for (var i = expandedStart; i <= expandedEnd; i++)
                {
                    var normalizedX = (float)i / (progressData.Length - 1);
                    var normalizedY = (float)progressData[i] / maxProgress;
                    points.Add(new Vector2(normalizedX, normalizedY));
                }

                result[state] = points;
            }

            return result;
        }

        private (int startIndex, int endIndex)? GetNonZeroRange(IReadOnlyList<int> data)
        {
            var startIndex = -1;
            var endIndex = -1;

            for (var i = 0; i < data.Count; i++)
            {
                if (data[i] <= 0)
                    continue;

                if (startIndex == -1)
                    startIndex = i;

                endIndex = i;
            }

            if (startIndex == -1)
                return null;

            return (startIndex, endIndex);
        }
    }
}