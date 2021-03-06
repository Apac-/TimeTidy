﻿using System.Collections.Generic;
using TimeTidy.Models;

namespace TimeTidy.Persistence.Repositories
{
    public interface IWorkSiteRepository
    {
        WorkSite GetWorkSite(int id);

        IEnumerable<WorkSite> GetWorkSites();

        WorkSite Add(WorkSite workSite);

        WorkSite Remove(WorkSite workSite);
    }
}
