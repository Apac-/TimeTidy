using System;
using System.Collections.Generic;
using TimeTidy.Models;

namespace TimeTidy.Persistance.Repositories
{
    public interface IApplicationUserRepository
    {
        ApplicationUser GetUser(string id);

        IEnumerable<ApplicationUser> GetUsers();
    }
}