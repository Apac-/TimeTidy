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

        IEnumerable<TimeSheet> GetTimeSheetsByWorkSite(int id);

        TimeSheet Remove(TimeSheet timeSheet);

        TimeSheet GetMostRecentSheetByUser(string userId, int? workSiteId);
    }
}