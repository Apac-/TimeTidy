using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        public TimeSheet Add(TimeSheet timeSheet)
        {
            return _context.TimeSheets.Add(timeSheet);
        }

        public TimeSheet Remove(TimeSheet timeSheet)
        {
            return _context.TimeSheets.Remove(timeSheet);
        }
    }
}