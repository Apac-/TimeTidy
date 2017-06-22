using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTidy.Models;

namespace TimeTidy.Services
{
    interface IApplicationUserManagerService
    {
        Task<ApplicationUser> FindUserByIdAsync(string userId);
        Task<IdentityResult> RemoveUserFromRolesAsync(string userId, params string[] roles);
        Task<IdentityResult> AddUserToRolesAsync(string userId, params string[] roles);
    }
}
