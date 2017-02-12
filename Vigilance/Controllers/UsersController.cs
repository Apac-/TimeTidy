﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            var roles = _context.Roles.ToList();

            if (userInDb == null)
                return HttpNotFound();

            var viewModel = new UserFormViewModel(userInDb, roles);

            return View("UserForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save()
        {
            if (!ModelState.IsValid)
            {
                // Recreate UserFOrmViewModel
                // Direction to UserForm
                throw new NotImplementedException();
            }

            // Get user from DB
            // Update with new information

            throw new NotImplementedException();
            return RedirectToAction("Index", "Users");
        }
    }
}