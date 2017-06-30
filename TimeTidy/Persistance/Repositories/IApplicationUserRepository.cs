using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeTidy.Models;

namespace TimeTidy.Persistance.Repositories
{
    public interface IApplicationUserRepository
    {
        ApplicationUser GetUser(string id);

        IList<string> GetRolesForUser(string id);

        IList<string> GetRoles();

        IEnumerable<ApplicationUser> GetUsers();

        IdentityResult DeleteUser(ApplicationUser user);

        Task<ApplicationUser> FindUserByIdAsync(string userId);

        Task<IdentityResult> RemoveUserFromRolesAsync(string userId, params string[] roles);

        Task<IdentityResult> AddUserToRolesAsync(string userId, params string[] roles);

        Task<IdentityResult> UpdateAsync(ApplicationUser user);
    }
}