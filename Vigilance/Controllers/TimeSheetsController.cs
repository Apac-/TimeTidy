using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Vigilance.Models;
using Vigilance.ViewModels;

namespace Vigilance.Controllers
{
    [Authorize(Roles = RoleName.CanManageWorkSites)]
    public class TimeSheetsController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        
        public TimeSheetsController()
        {
            _context = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }


        // GET: TimeSheets
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(string id)
        {
            var userInDb = _context.Users.SingleOrDefault(u => u.Id == id);

            var vm = new TimeSheetsListViewModel
            {
                UserId = id,
                UserName = userInDb.UserName
            };

            return View(vm);
        }
    }
}