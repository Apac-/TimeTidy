using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TimeTidy.Models;

namespace TimeTidy.Controllers
{
    public class WorkSitesController : Controller
    {
        private ApplicationDbContext _context;

        public WorkSitesController() {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing) {
            _context.Dispose();
            base.Dispose(disposing);
        }

        [Authorize(Roles = RoleName.CanManageWorkSites)]
        public ActionResult Edit(int id)
        {
            var worksite = _context.WorkSites.SingleOrDefault(w => w.Id == id);

            if (worksite == null)
                return HttpNotFound();

            return View("WorkSiteForm", worksite);
        }

        [Authorize(Roles = RoleName.CanManageWorkSites)]
        public ActionResult New() {
            var workSite = new WorkSite();

            return View("WorkSiteForm", workSite);
        }

        [HttpPost]
        [Authorize(Roles = RoleName.CanManageWorkSites)]
        public ActionResult Save(WorkSite worksite) {
            if (!ModelState.IsValid)
                return View("WorkSiteForm", worksite);

            if (worksite.Id == 0)
            {
                _context.WorkSites.Add(worksite);
            }
            else
            {
                var worksiteInDb = _context.WorkSites.Single(w => w.Id == worksite.Id);
                worksiteInDb.Name = worksite.Name;
                worksiteInDb.Description = worksite.Description;
                worksiteInDb.StreetAddress = worksite.StreetAddress;
                worksiteInDb.Lat = worksite.Lat;
                worksiteInDb.Lng = worksite.Lng;
                worksiteInDb.Radius = worksite.Radius;
            }

            _context.SaveChanges();


            return RedirectToAction("List", "WorkSites");
        }

        // GET: WorkSite
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = RoleName.CanManageWorkSites)]
        public ActionResult List()
        {
            var sites = _context.WorkSites.ToList();

            return View(sites);
        }
    }
}