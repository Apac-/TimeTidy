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

        [HttpPost]
        public ActionResult Save(WorkSite worksite) {
            // TODO (Jeff): B. Add vailidation
            if (worksite.Id == 0)
            {
                _context.WorkSites.Add(worksite);
            }
            else
            {
                // TODO (Jeff): B. Change this to DTO and mapper
                var worksiteInDB = _context.WorkSites.Single(w => w.Id == worksite.Id);
                worksiteInDB.Name = worksite.Name;
                worksiteInDB.Description = worksite.Description;
                worksiteInDB.Lat = worksite.Lat;
                worksiteInDB.Lng = worksite.Lng;
                worksiteInDB.Radius = worksite.Radius;
            }

            return RedirectToAction("Index", "WorkSite");
        }

        // GET: WorkSite
        public ActionResult Index()
        {
            var sites = _context.WorkSites.ToList();

            return View(sites);
        }
    }
}