using System.Collections.Generic;
using TimeTidy.Models;

namespace TimeTidy.Persistance.Repositories
{
    public interface ITimeSheetRepository
    {
        TimeSheet Add(TimeSheet timeSheet);

        TimeSheet GetTimeSheet(int id);

        IEnumerable<TimeSheet> GetTimeSheets();

        TimeSheet Remove(TimeSheet timeSheet);
    }
}