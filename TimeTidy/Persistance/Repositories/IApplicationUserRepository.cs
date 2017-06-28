using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using TimeTidy.Models;

namespace TimeTidy.Persistance.Repositories
{
    public interface IApplicationUserRepository
    {
        ApplicationUser GetUser(string id);

        IEnumerable<IdentityRole> GetRolesForUser(string id);

        IEnumerable<ApplicationUser> GetUsers();

        IEnumerable<IdentityRole> GetRoles();
    }
}