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

        public ActionResult List(string id)
        {
            var vm = new TimeSheetsListViewModel
            {
                UserId = id
            };

            return View(vm);
        }
    }
}