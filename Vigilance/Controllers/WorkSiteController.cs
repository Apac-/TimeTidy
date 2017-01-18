using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vigilance.Models;

namespace Vigilance.Controllers
{
    public class WorkSiteController : Controller
    {
        private ApplicationDbContext _context;

        public WorkSiteController() {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing) {
            _context.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Edit() {
            var sites = _context.WorkSites.ToList();

            return View(sites);
        }

        // GET: WorkSite
        public ActionResult Index()
        {
            var sites = _context.WorkSites.ToList();

            return View(sites);
        }
    }
}