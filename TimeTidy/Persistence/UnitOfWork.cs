using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeTidy.Models;
using TimeTidy.Persistence.Repositories;

namespace TimeTidy.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public IApplicationUserRepository Users { get; private set; }

        public IWorkSiteRepository WorkSites { get; private set; }

        public ITimeSheetRepository TimeSheets { get; private set; }

        public UnitOfWork(ApplicationDbContext context, ApplicationUserManager userManager)
        {
            _context = context;
            Users = new ApplicationUserRepository(context, userManager);
            WorkSites = new WorkSiteRepository(context);
            TimeSheets = new TimeSheetRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}