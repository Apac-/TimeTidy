using System;
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

            if (userInDb == null)
                return HttpNotFound();

            return View("UserForm", userInDb);
        }
    }
}