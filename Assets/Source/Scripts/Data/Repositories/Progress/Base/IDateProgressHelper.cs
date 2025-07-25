using Source.Scripts.Data.Repositories.Progress.Entries;
using Source.Scripts.Data.Repositories.Vocabulary.Entries;

namespace Source.Scripts.Data.Repositories.Progress.Base
{
    internal interface IDateProgressHelper
    {
        DailyProgress[] GetCurrentWeek();
        (DailyProgress[] days, bool[] isInMonth) GetMonthWeeks(int year, int month);
        int GetProgressForRange(int daysBack, int daysDuration, LearningState learningState);
    }
}