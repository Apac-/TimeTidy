using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeTidy.Models;
using TimeTidy.Services;

namespace TimeTidy.Controllers
{
    [Authorize(Roles = RoleName.CanManageUsers)]
    public class UsersController : Controller
    {
        private IDbContextService _context;
        private IApplicationUserManagerService _userManager;

        public UsersController(IDbContextService contextService, IApplicationUserManagerService userManager)
        {
            _context = contextService;
            _userManager = userManager;
        }

        // GET: Users
        public ActionResult Index()
        {
            var users = _context.Users();

            var admin = users.First(u => u.UserName.Contains("admin"));
            if (admin != null)
                users.Remove(admin);

            return View("Index", users);
        }

        public ActionResult Edit(string id)
        {
            var userInDb = _context.FindUserOrDefault(id);

            if (userInDb == null)
                return HttpNotFound();

            var roles = _context.Roles();

            var userRoles = roles.Where(r => userInDb.Roles.Any(u => r.Id == u.RoleId)).ToList();

            var viewModel = new UserFormViewModel(userInDb, roles, userRoles);

            return View("UserForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(User user)
        {
            if (!ModelState.IsValid)
            {
                var uInDb = _context.FindUserOrDefault(user.UserId);
                if (uInDb == null)
                    return HttpNotFound();

                var roles = _context.Roles();
                var userRoles = roles.Where(r => uInDb.Roles.Any(u => r.Id == u.RoleId)).ToList();
                var viewModel = new UserFormViewModel(uInDb, roles, userRoles);
                return View("UserForm", viewModel);
            }

            // Get user from DB
            // Update with new information
            var userInDb = _context.FindUser(user.UserId);
            userInDb.FirstName = user.FirstName;
            userInDb.LastName = user.LastName;
            userInDb.PhoneNumber = user.PhoneNumber;
            userInDb.UserName = user.Email;
            userInDb.Email = user.Email;

            // Remove all roles then add new ones
            var userInManager = await _userManager.FindUserByIdAsync(user.UserId);
            if (userInManager.Roles != null && userInManager.Roles.Count > 0)
                await _userManager.RemoveUserFromRolesAsync(user.UserId, _userManager.GetRolesForUser(user.UserId));
            if (user.UserRoles != null && user.UserRoles.Count > 0)
                await _userManager.AddUserToRolesAsync(user.UserId, user.UserRoles.ToArray());
            await _userManager.UpdateAsync(userInManager);

            _context.SaveChanges();

            return RedirectToAction("Index", "Users");
        }
    }
}