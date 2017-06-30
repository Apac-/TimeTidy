using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeTidy.Models;

namespace TimeTidy.Persistance.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly IApplicationDbContext _context;
        private ApplicationUserManager _userManager;

        public ApplicationUserRepository(IApplicationDbContext context, ApplicationUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<string> GetRoles()
        {
            return _context.Roles.Select(r => r.Name).ToArray();
        }

        public IList<string> GetRolesForUser(string id)
        {
            var user = GetUser(id);

            var roles = _context.Roles
                .Where(r => user.Roles.Any(u => r.Id == u.RoleId))
                .Select(r => r.Name)
                .ToArray();

            return roles;
        }

        public ApplicationUser GetUser(string id)
        {
            return _context.Users.Single(u => u.Id == id);
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _context.Users.ToList();
        }

        public IdentityResult DeleteUser(ApplicationUser user)
        {
            return _userManager.Delete(user);
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

        public Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return _userManager.UpdateAsync(user);
        }
    }
}