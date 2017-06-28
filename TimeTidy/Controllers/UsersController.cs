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
using TimeTidy.Persistance;

namespace TimeTidy.Controllers
{
    [Authorize(Roles = RoleName.CanManageUsers)]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IApplicationUserManagerService _userManager;

        public UsersController(IUnitOfWork unitOfWork, IApplicationUserManagerService userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        // GET: Users
        public ActionResult Index()
        {
            var users = _unitOfWork.Users.GetUsers();

            return View("Index", users);
        }

        public ActionResult Edit(string id)
        {
            var userInDb = _unitOfWork.Users.GetUser(id);

            if (userInDb == null)
                return HttpNotFound();

            var roles = _unitOfWork.Users.GetRoles();

            var userRoles = _unitOfWork.Users.GetRolesForUser(userInDb.Id);

            var viewModel = new UserFormViewModel(userInDb, roles, userRoles);

            return View("UserForm", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Save(User user)
        {
            if (!ModelState.IsValid)
            {
                var uInDb = _unitOfWork.Users.GetUser(user.UserId);
                if (uInDb == null)
                    return HttpNotFound();

                var roles = _unitOfWork.Users.GetRoles();
                var userRoles = roles.Where(r => uInDb.Roles.Any(u => r.Id == u.RoleId)).ToList();
                var viewModel = new UserFormViewModel(uInDb, roles, userRoles);
                return View("UserForm", viewModel);
            }

            // Get user from DB
            // Update with new information
            var userInDb = _unitOfWork.Users.GetUser(user.UserId);
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

            _unitOfWork.Complete();

            return RedirectToAction("Index", "Users");
        }
    }
}