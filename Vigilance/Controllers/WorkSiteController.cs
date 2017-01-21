﻿using System;
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

        public ActionResult Edit(int id)
        {
            var worksite = _context.WorkSites.SingleOrDefault(w => w.Id == id);

            if (worksite == null)
                return HttpNotFound();

            return View("WorkSiteForm", worksite);
        }

        public ActionResult New() {
            var workSite = new WorkSite();

            return View("WorkSiteForm", workSite);
        }

        [HttpPost]
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


            return RedirectToAction("Index", "WorkSite");
        }

        // GET: WorkSite
        public ActionResult Index()
        {
            var sites = _context.WorkSites.ToList();

            return View("List", sites);
        }
    }
}