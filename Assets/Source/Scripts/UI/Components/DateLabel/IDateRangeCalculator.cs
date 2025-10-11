namespace Source.Scripts.UI.Components.DateLabel
{
    internal interface IDateRangeCalculator
    {
        DateRangeData Calculate(DateRange dateRange, int pointsCount);
    }
}