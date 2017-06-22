using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using TimeTidy.Models;

namespace TimeTidy.Services
{
    public class ApplicationUserManagerService : IApplicationUserManagerService
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManagerService(ApplicationUserManager applicationUserManager)
        {
            _userManager = applicationUserManager;
        }

        public Task<IdentityResult> AddUserToRolesAsync(string userId, params string[] roles)
        {
            return _userManager.AddToRolesAsync(userId, roles);
        }

        public Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            return _userManager.FindByIdAsync(userId);
        }

        public Task<IdentityResult> RemoveUserFromRolesAsync(string userId, params string[] roles)
        {
            return _userManager.RemoveFromRolesAsync(userId, roles);
        }
    }
}