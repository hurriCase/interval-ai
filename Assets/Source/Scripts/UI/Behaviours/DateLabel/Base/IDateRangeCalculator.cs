using System;

namespace Source.Scripts.UI.Behaviours.DateLabel.Base
{
    internal interface IDateRangeCalculator
    {
        DateTime[] Calculate(int totalDays, int pointsCount);
    }
}