using System.Data.Entity;

namespace TimeTidy.Models
{
    // TODO (Jeff): B. Move to presistance folder
    public interface IApplicationDbContext
    {
        DbSet<TimeSheet> TimeSheets { get; set; }

        DbSet<WorkSite> WorkSites { get; set; }

        IDbSet<ApplicationUser> Users { get; set; }
    }
}