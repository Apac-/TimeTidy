using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TimeTidy.Models;
using TimeTidy.Models.DTOs;
using TimeTidy.Persistence;

namespace TimeTidy.Controllers.Api
{
    [Authorize(Roles = RoleName.CanManageUsers)]
    public class UsersController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET /api/users
        public IHttpActionResult GetUsers()
        {
            var usersInDb = _unitOfWork.Users.GetUsers();

            var usersDto = usersInDb.Select(user => new BasicUsersDTO
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
            var userInDb = _unitOfWork.Users.GetUser(id);

            if (userInDb == null)
                return NotFound();

            return Ok(userInDb);
        }

        // DELETE /api/users/id
        public IHttpActionResult DeleteUser(string id)
        {
            // TODO (Jeff): A. Check to make sure at least 1 account can still manage users!

            var userInDb = _unitOfWork.Users.GetUser(id);
            if (userInDb == null)
                return NotFound();

            _unitOfWork.Users.DeleteUser(userInDb);

            _unitOfWork.Complete();

            return Ok();
        }
    }
}
