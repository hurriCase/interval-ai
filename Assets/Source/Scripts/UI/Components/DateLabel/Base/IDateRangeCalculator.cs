namespace Source.Scripts.UI.Components.DateLabel.Base
{
    internal interface IDateRangeCalculator
    {
        DateRangeData Calculate(DateRange dateRange, int pointsCount);
    }
}