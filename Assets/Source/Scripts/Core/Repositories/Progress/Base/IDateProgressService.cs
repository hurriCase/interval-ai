using Source.Scripts.Core.Repositories.Words.Base;

namespace Source.Scripts.Core.Repositories.Progress.Base
{
    internal interface IDateProgressService
    {
        DailyProgress[] GetCurrentWeek();
        (DailyProgress[] days, bool[] isInMonth) GetMonthWeeks(int year, int month);
        int GetProgressForRange(int daysBack, int daysDuration, LearningState learningState);
    }
}