using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using Source.Scripts.Core.Repositories.Words.Base;
using UnityEngine;

namespace Source.Scripts.Main.UI.PopUps.Achievement.Behaviours.LearningStarts.GraphProgress
{
    internal interface IGraphDataProcessor
    {
        int MaxProgress { get; }
        EnumArray<LearningState, List<Vector2>> NormalizedPoints { get; }
        void GetDisplayGraphData(int totalDays, int pointsCount);
    }
}