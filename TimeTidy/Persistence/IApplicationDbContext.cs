using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using TimeTidy.Models;

namespace TimeTidy.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<TimeSheet> TimeSheets { get; set; }

        DbSet<WorkSite> WorkSites { get; set; }

        IDbSet<ApplicationUser> Users { get; set; }

        IDbSet<IdentityRole> Roles { get; set; }
    }
}