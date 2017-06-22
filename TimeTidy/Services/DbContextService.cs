using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeTidy.Models;

namespace TimeTidy.Services
{
    public class DbContextService : IDbContextService
    {
        private ApplicationDbContext _context;

        public DbContextService()
        {
            _context = new ApplicationDbContext();
        }

        public ApplicationUser FindUser(string id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }

        public WorkSite FindWorkSite(int id)
        {
            return _context.WorkSites.SingleOrDefault(w => w.Id == id);
        }
    }
}