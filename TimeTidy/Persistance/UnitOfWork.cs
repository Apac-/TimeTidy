using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeTidy.Models;
using TimeTidy.Persistance.Repositories;

namespace TimeTidy.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public IApplicationUserRepository Users { get; private set; }

        public IWorkSiteRepository WorkSites { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new ApplicationUserRepository(context);
            WorkSites = new WorkSiteRepository(context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}