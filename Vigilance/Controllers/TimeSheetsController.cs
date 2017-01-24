using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vigilance.Models;
using Vigilance.ViewModels;

namespace Vigilance.Controllers
{
    public class TimeSheetsController : Controller
    {
        private ApplicationDbContext _context;

        public TimeSheetsController()
        {
            _context = new ApplicationDbContext();
        }


        // GET: TimeSheets
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserTimeSheets(string userId)
        {
            var timeSheets = _context.TimeSheets.Where(s => s.ApplicationUserId == userId);

            List<UserTimeSheetViewModel> vm = new List<UserTimeSheetViewModel>();

            foreach (var timeSheet in timeSheets)
            {
                vm.Add(new UserTimeSheetViewModel(timeSheet));
            }

            return View(vm);
        }
    }
}