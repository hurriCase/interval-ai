using System;
using System.Collections.Generic;
using System.Linq;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Progress.Base;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.UI.Components.DateLabel;
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

        public GraphDisplayData GetDisplayGraphData(DateRange dateRange, int pointsCount)
        {
            var rawGraphData = GetGraphDataForRange(dateRange, pointsCount);
            var maxProgress = CalculateMaxProgress(rawGraphData);
            var normalizedData = NormalizeAllData(rawGraphData, maxProgress);

            return new GraphDisplayData(maxProgress, normalizedData);
        }

        private EnumArray<LearningState, int[]> GetGraphDataForRange(DateRange dateRange, int pointsCount)
        {
            var totalDays = dateRange.GetDayCount();
            var daysPerSegment = (float)totalDays / pointsCount;
            var graphData = new EnumArray<LearningState, int[]>(EnumMode.SkipFirst);

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

        private static int CalculateMaxProgress(EnumArray<LearningState, int[]> graphData)
        {
            var maxProgress = 0;

            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator | Due to boxing
            foreach (var progressData in graphData)
                maxProgress = Math.Max(maxProgress, progressData.Max());

            return maxProgress;
        }

        private static EnumArray<LearningState, List<Vector2>> NormalizeAllData(
            EnumArray<LearningState, int[]> rawData,
            int maxProgress)
        {
            var result = new EnumArray<LearningState, List<Vector2>>(EnumMode.SkipFirst);

            foreach (var (state, progressData) in rawData.AsTuples())
                result[state] = NormalizePoints(progressData, maxProgress);

            return result;
        }

        private static List<Vector2> NormalizePoints(IReadOnlyList<int> progressData, int maxProgress)
        {
            var points = new List<Vector2>(progressData.Count);

            if (maxProgress <= 0)
                return points;

            for (var i = 0; i < progressData.Count; i++)
            {
                var normalizedX = (float)i / (progressData.Count - 1);
                var normalizedY = (float)progressData[i] / maxProgress;
                points.Add(new Vector2(normalizedX, normalizedY));
            }

            return points;
        }
    }
}