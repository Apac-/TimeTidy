﻿using System;
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
    [Authorize(Roles = RoleName.CanManageUsers)]
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

            // If built in admin account then skip. Prevents del of master account
            var admin = users.First(u => u.UserName =="admin@timetidy.com");
            if (admin != null)
                users.Remove(admin);

            var usersDto = users.Select(user => new BasicUsersDTO
            {
                UserName = user.UserName, 
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            }).ToList();

            return Ok(usersDto);
        }

        // Get /api/users/id
        public IHttpActionResult GetUsers(string id)
        {
            // TODO (Jeff): A. Add in db call once TimeSheets are created.
            //  Send TimeSheets with a BasicUsersDto, or equivilant
            var user = _userManager.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // DELETE /api/users/id
        public IHttpActionResult DeleteUser(string id)
        {
            var userInDb = _userManager.FindById(id);
            if (userInDb == null)
                return NotFound();

            _userManager.Delete(userInDb);

            return Ok();
        }
    }
}
