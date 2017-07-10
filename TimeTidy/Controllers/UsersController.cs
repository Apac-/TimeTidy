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
using TimeTidy.Persistence;

namespace TimeTidy.Controllers
{
    [Authorize(Roles = RoleName.CanManageUsers)]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get: Users
        /// Returns all users in context
        /// </summary>
        /// <returns>All users in context</returns>
        public ActionResult Index()
        {
            var users = _unitOfWork.Users.GetUsers();

            return View("Index", users);
        }

        /// <summary>
        /// Shows page to edit user of given id.
        /// </summary>
        /// <param name="id">User Id</param>
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

        /// <summary>
        /// Update user details for given User
        /// </summary>
        /// <param name="user">Given user</param>
        /// <returns>Redirect to user index</returns>
        [HttpPost]
        public async Task<ActionResult> Save(User user)
        {
            if (!ModelState.IsValid)
            {
                var uInDb = _unitOfWork.Users.GetUser(user.UserId);

                if (uInDb == null)
                    return HttpNotFound();

                var roles = _unitOfWork.Users.GetRoles();
                var userRoles = _unitOfWork.Users.GetRolesForUser(uInDb.Id);
                var viewModel = new UserFormViewModel(uInDb, roles, userRoles);

                return View("UserForm", viewModel);
            }

            // Get user from DB
            // Update with new information
            var userInDb = _unitOfWork.Users.GetUser(user.UserId);

            if (userInDb == null)
                return HttpNotFound();

            userInDb.FirstName = user.FirstName;
            userInDb.LastName = user.LastName;
            userInDb.PhoneNumber = user.PhoneNumber;
            userInDb.UserName = user.Email;
            userInDb.Email = user.Email;

            // Remove all roles then add new ones
            var userInManager = await _unitOfWork.Users.FindUserByIdAsync(user.UserId);

            if (userInManager.Roles != null && userInManager.Roles.Count > 0)
                await _unitOfWork.Users.RemoveUserFromRolesAsync(user.UserId, _unitOfWork.Users.GetRolesForUser(user.UserId).ToArray());

            if (user.UserRoles != null && user.UserRoles.Count > 0)
                await _unitOfWork.Users.AddUserToRolesAsync(user.UserId, user.UserRoles.ToArray());

            await _unitOfWork.Users.UpdateAsync(userInManager);

            _unitOfWork.Complete();

            return RedirectToAction("Index", "Users");
        }
    }
}