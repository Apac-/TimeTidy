using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Vigilance.Models;
using Vigilance.Models.DTOs;

namespace Vigilance.Controllers.Api
{
    public class UsersController : ApiController
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UsersController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
        }

        // GET /api/users
        public IHttpActionResult GetUsers()
        {
            var users = _userManager.Users.ToList();

            var usersDto = users.Select(user => new UsersDTO
            {
                UserName = user.UserName, Id = user.Id
            }).ToList();

            return Ok(usersDto);
        }
    }
}
