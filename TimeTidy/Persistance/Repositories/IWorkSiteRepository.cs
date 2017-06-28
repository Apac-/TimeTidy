using System.Collections.Generic;
using TimeTidy.Models;

namespace TimeTidy.Persistance.Repositories
{
    public interface IWorkSiteRepository
    {
        WorkSite GetWorkSite(int id);

        IEnumerable<WorkSite> GetWorkSites();

        WorkSite Add(WorkSite workSite);
    }
}
