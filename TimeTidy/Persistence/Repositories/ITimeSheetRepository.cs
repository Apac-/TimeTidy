using System.Collections.Generic;
using TimeTidy.Models;

namespace TimeTidy.Persistence.Repositories
{
    public interface ITimeSheetRepository
    {
        TimeSheet Add(TimeSheet timeSheet);

        TimeSheet GetTimeSheet(int id);

        IEnumerable<TimeSheet> GetTimeSheets();

        IEnumerable<TimeSheet> GetTimeSheetsByUser(string id);

        IEnumerable<TimeSheet> GetTimeSheetsByWorkSite(int id);

        TimeSheet Remove(TimeSheet timeSheet);

        TimeSheet GetCurrentLoggedInSheetForUser(string userId, int? workSiteId);
    }
}