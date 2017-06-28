using System.Collections.Generic;
using TimeTidy.Models;

namespace TimeTidy.Persistance.Repositories
{
    public interface ITimeSheetRepository
    {
        TimeSheet Add(TimeSheet timeSheet);

        TimeSheet GetTimeSheet(int id);

        IEnumerable<TimeSheet> GetTimeSheets();

        IEnumerable<TimeSheet> GetTimeSheetsByUser(string id);

        TimeSheet Remove(TimeSheet timeSheet);
    }
}