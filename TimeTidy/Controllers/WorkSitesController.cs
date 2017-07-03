using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeTidy.Models;
using TimeTidy.Persistence;

namespace TimeTidy.Controllers
{
    public class WorkSitesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkSitesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = RoleName.CanManageWorkSites)]
        public ActionResult Edit(int id)
        {
            var worksite = _unitOfWork.WorkSites.GetWorkSite(id);

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
                _unitOfWork.WorkSites.Add(worksite);
            }
            else
            {
                var worksiteInDb = _unitOfWork.WorkSites.GetWorkSite(worksite.Id);
                worksiteInDb.Name = worksite.Name;
                worksiteInDb.Description = worksite.Description;
                worksiteInDb.StreetAddress = worksite.StreetAddress;
                worksiteInDb.Lat = worksite.Lat;
                worksiteInDb.Lng = worksite.Lng;
                worksiteInDb.Radius = worksite.Radius;
            }

            _unitOfWork.Complete();

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
            var sites = _unitOfWork.WorkSites.GetWorkSites();

            return View(sites);
        }
    }
}