using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TimeTidy.Models;
using TimeTidy.Services;

namespace TimeTidy.Controllers
{
    public class WorkSitesController : Controller
    {
        private IDbContextService _context;

        public WorkSitesController(IDbContextService contextService)
        {
            _context = contextService;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        [Authorize(Roles = RoleName.CanManageWorkSites)]
        public ActionResult Edit(int id)
        {
            var worksite = _context.FindWorkSiteOrDefault(id);

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
                _context.AddWorkSite(worksite);
            }
            else
            {
                var worksiteInDb = _context.FindWorkSite(worksite.Id);
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
            var sites = _context.WorkSites();

            return View(sites);
        }
    }
}