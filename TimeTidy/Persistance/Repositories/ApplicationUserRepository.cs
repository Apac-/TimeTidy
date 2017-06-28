using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeTidy.Models;

namespace TimeTidy.Persistance.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly IApplicationDbContext _context;

        public ApplicationUserRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }

        public IEnumerable<IdentityRole> GetRolesForUser(string id)
        {
            var user = GetUser(id);
            return _context.Roles.Where(r => user.Roles.Any(u => r.Id == u.RoleId)).ToList();
        }

        public ApplicationUser GetUser(string id)
        {
            return _context.Users.Single(u => u.Id == id);
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _context.Users.ToList();
        }
    }
}