using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using TimeTidy.Models;

namespace TimeTidy.Persistance.Repositories
{
    public class TimeSheetRepository : ITimeSheetRepository
    {
        private readonly IApplicationDbContext _context;

        public TimeSheetRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public TimeSheet GetTimeSheet(int id)
        {
            return _context.TimeSheets.SingleOrDefault(t => t.Id == id);
        }

        public IEnumerable<TimeSheet> GetTimeSheets()
        {
            return _context.TimeSheets.ToList();
        }

        public IEnumerable<TimeSheet> GetTimeSheetsByUser(string id)
        {
            return _context.TimeSheets
                .Include(t => t.SiteLocation)
                .Include(t => t.LogOnLocation)
                .Include(t => t.LogOffLocation)
                .Where(t => t.ApplicationUserId == id).ToList();
        }

        public IEnumerable<TimeSheet> GetTimeSheetsByWorkSite(int id)
        {
            return _context.TimeSheets
                .Include(t => t.SiteLocation)
                .Include(t => t.LogOnLocation)
                .Include(t => t.LogOffLocation)
                .Where(t => t.WorkSiteId == id).ToList();
        }

        public TimeSheet Add(TimeSheet timeSheet)
        {
            return _context.TimeSheets.Add(timeSheet);
        }

        public TimeSheet Remove(TimeSheet timeSheet)
        {
            return _context.TimeSheets.Remove(timeSheet);
        }

        public TimeSheet GetCurrentLoggedInSheetForUser(string userId, int? workSiteId = null)
        {
            var sheets = _context.TimeSheets.Where(s => s.ApplicationUserId == userId);

            if (workSiteId != null)
                sheets = sheets.Where(s => s.WorkSiteId == workSiteId);

            // Get only sheet where they haven't logged off yet
            sheets = sheets.Where(s => s.LogOffTime == null);

            var sheet = sheets.OrderByDescending(t => t.LogOnTime).FirstOrDefault();

            return sheet;
        }
    }
}