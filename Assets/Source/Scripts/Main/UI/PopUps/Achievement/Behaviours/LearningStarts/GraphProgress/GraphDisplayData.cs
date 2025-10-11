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

        internal GraphDisplayData(int maxProgress, EnumArray<LearningState, List<Vector2>> normalizedPoints)
        {
            MaxProgress = maxProgress;
            NormalizedPoints = normalizedPoints;
        }
    }
}