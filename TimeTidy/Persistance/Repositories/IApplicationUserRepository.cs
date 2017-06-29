using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTidy.Models;

namespace TimeTidy.Persistance.Repositories
{
    public interface IApplicationUserRepository
    {
        ApplicationUser GetUser(string id);

        // TODO (Jeff): B. Get rid of IdentityRole which removes using EntityFramework
        //      Collection of strings should work.
        IEnumerable<IdentityRole> GetRolesForUser(string id);

        IEnumerable<ApplicationUser> GetUsers();

        IEnumerable<IdentityRole> GetRoles();

        IdentityResult DeleteUser(ApplicationUser user);

        Task<ApplicationUser> FindUserByIdAsync(string userId);

        Task<IdentityResult> RemoveUserFromRolesAsync(string userId, params string[] roles);

        Task<IdentityResult> AddUserToRolesAsync(string userId, params string[] roles);

        Task<IdentityResult> UpdateAsync(ApplicationUser user);
    }
}