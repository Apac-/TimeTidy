using System;
using System.Collections.Generic;
using TimeTidy.Persistence.Repositories;

namespace TimeTidy.Persistence
{
    public interface IUnitOfWork
    {
        IWorkSiteRepository WorkSites { get; }

        IApplicationUserRepository Users { get; }

        ITimeSheetRepository TimeSheets { get; }

        void Complete();
    }
}
