using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTidy.Models;
using TimeTidy.ViewModels;
using TimeTidy.Persistence;

namespace TimeTidy.Controllers
{
    [Authorize(Roles = RoleName.CanManageWorkSites)]
    public class TimeSheetsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TimeSheetsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: TimeSheets
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns ViewResult of user id and name for given user by Id.
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>ViewResult of userId and Name</returns>
        public ActionResult List(string id)
        {
            var userInDb = _unitOfWork.Users.GetUser(id);

            var vm = new TimeSheetsListViewModel
            {
                UserId = id,
                UserName = userInDb.UserName
            };

            return View(vm);
        }

        public ActionResult Worksite(int id)
        {
            var siteInDb = _unitOfWork.WorkSites.GetWorkSite(id);

            if (siteInDb == null)
                return HttpNotFound();

            return View("worksite", siteInDb);
        }
    }
}