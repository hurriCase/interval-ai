using System;
using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal readonly struct GraphDisplayData
    {
        internal int MaxProgress { get; }
        internal EnumArray<LearningState, List<Vector2>> NormalizedPoints { get; }
        internal DateTime[] RangeData { get; }

        internal GraphDisplayData(
            int maxProgress,
            EnumArray<LearningState, List<Vector2>> normalizedPoints,
            DateTime[] dateRange)
        {
            MaxProgress = maxProgress;
            NormalizedPoints = normalizedPoints;
            RangeData = dateRange;
        }

        internal static GraphDisplayData Empty =>
            new(0,
                new EnumArray<LearningState, List<Vector2>>(EnumMode.SkipFirst),
                Array.Empty<DateTime>());
    }
}