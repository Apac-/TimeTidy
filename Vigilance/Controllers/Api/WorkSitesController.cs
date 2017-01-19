using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vigilance.Models;

namespace Vigilance.Controllers.Api
{
    public class WorkSitesController : ApiController
    {
        private ApplicationDbContext _context;

        public WorkSitesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/worksites
        public IEnumerable<WorkSite> GetWorkSites()
        {
            return _context.WorkSites.ToList();
        }

        // GET /api/worksites/1
        public WorkSite GetWorkSite(int id)
        {
            var worksite = _context.WorkSites.SingleOrDefault(w => w.Id == id);

            if (worksite == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return worksite;
        }

        // POST /api/worksites
        [HttpPost]
        public WorkSite CreateWorkSite(WorkSite worksite)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _context.WorkSites.Add(worksite);
            _context.SaveChanges();

            return worksite;
        }

        // PUT /api/worksites/1
        [HttpPut]
        public void UpdateWorkSite(int id, WorkSite worksite)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var siteInDb = _context.WorkSites.SingleOrDefault(w => w.Id == id);

            if (siteInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            // TODO (Jeff): B. Use mapper here
            siteInDb.Name = worksite.Name;
            siteInDb.Description = worksite.Description;
            siteInDb.Lat = worksite.Lat;
            siteInDb.Lng = worksite.Lng;
            siteInDb.Radius = worksite.Radius;

            _context.SaveChanges();
        }

        // DELET /api/worksites/1
        [HttpDelete]
        public void DeleteWorkSite(int id)
        {
            var siteInDb = _context.WorkSites.SingleOrDefault(w => w.Id == id);

            if (siteInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.WorkSites.Remove(siteInDb);
            _context.SaveChanges();
        }
    }
}
