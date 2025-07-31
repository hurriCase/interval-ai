using Source.Scripts.Core.DI.Repositories.Words.Base;

namespace Source.Scripts.Core.DI.Repositories.Progress.Base
{
    internal interface IDateProgressHelper
    {
        DailyProgress[] GetCurrentWeek();
        (DailyProgress[] days, bool[] isInMonth) GetMonthWeeks(int year, int month);
        int GetProgressForRange(int daysBack, int daysDuration, LearningState learningState);
    }
}