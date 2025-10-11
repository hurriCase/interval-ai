using Source.Scripts.UI.Behaviours.DateLabel.Range;

namespace Source.Scripts.UI.Behaviours.DateLabel.Base
{
    internal interface IDateRangeCalculator
    {
        DateRangeData Calculate(DateRange dateRange, int pointsCount);
    }
}