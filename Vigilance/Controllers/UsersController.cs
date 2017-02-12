﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Vigilance.Models;

namespace Vigilance.Controllers
{
    public class UsersController : Controller
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

        // GET: Users
        public ActionResult Index()
        {
            var users = _userManager.Users.ToList();

            return View("Index", users);
        }

        public ActionResult Edit(string id)
        {
            var userInDb = _userManager.Users.SingleOrDefault(u => u.Id == id);

            if (userInDb == null)
                return HttpNotFound();

            var roles = _context.Roles.ToList();

            var userRoles = roles.Where(r => userInDb.Roles.Any(u => r.Id == u.RoleId)).ToList();

            var viewModel = new UserFormViewModel(userInDb, roles, userRoles);

            return View("UserForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(User user)
        {
            if (!ModelState.IsValid)
            {
                var uInDb = _userManager.Users.SingleOrDefault(u => u.Id == user.Id);
                if (uInDb == null)
                    return HttpNotFound();

                var roles = _context.Roles.ToList();
                var userRoles = roles.Where(r => uInDb.Roles.Any(u => r.Id == u.RoleId)).ToList();
                var viewModel = new UserFormViewModel(uInDb, roles, userRoles);
                return View("UserForm", viewModel);
            }

            // Get user from DB
            // Update with new information
            var userInDb = _userManager.Users.Single(u => u.Id == user.Id);
            userInDb.FirstName = user.FirstName;
            userInDb.LastName = user.LastName;
            userInDb.PhoneNumber = user.PhoneNumber;
            userInDb.UserName = user.Email;
            userInDb.Email = user.Email;

            // Remove all roles then add new ones
            var userInManager = await _userManager.FindByIdAsync(user.Id);
            await _userManager.RemoveFromRolesAsync(user.Id, _userManager.GetRoles(user.Id).ToArray());
            await _userManager.AddToRolesAsync(user.Id, user.UserRoles.ToArray());
            await _userManager.UpdateAsync(userInManager);

            return RedirectToAction("Index", "Users");
        }
    }
}