using System;
using System.Collections.Generic;
using TimeTidy.Persistance.Repositories;

namespace TimeTidy.Persistance
{
    public interface IUnitOfWork
    {
        IWorkSiteRepository WorkSite { get; }

        IApplicationUserRepository Users { get; }

        void Complete();
    }
}
