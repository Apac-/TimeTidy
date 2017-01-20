using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Vigilance.DTOs;
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
        public IHttpActionResult GetWorkSites()
        {
            //return Ok(_context.WorkSites.ToList().Select(Mapper.Map<WorkSite, WorkSiteDTO>));
            return Ok(_context.WorkSites.ToList());
        }

        // GET /api/worksites/1
        public IHttpActionResult GetWorkSite(int id)
        {
            var worksite = _context.WorkSites.SingleOrDefault(w => w.Id == id);

            if (worksite == null)
                return NotFound();

            //return Ok(Mapper.Map<WorkSite, WorkSiteDTO>(worksite));
            return Ok(worksite);
        }

        // POST /api/worksites
        [HttpPost]
        public IHttpActionResult CreateWorkSite(WorkSiteDTO workSiteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var workSite = Mapper.Map<WorkSiteDTO, WorkSite>(workSiteDto);

            _context.WorkSites.Add(workSite);
            _context.SaveChanges();

            workSiteDto.Id = workSite.Id;

            return Created(new Uri(Request.RequestUri + "/" + workSite.Id), workSiteDto);
        }

        // PUT /api/worksites/1
        [HttpPut]
        public IHttpActionResult UpdateWorkSite(int id, WorkSiteDTO workSiteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var siteInDb = _context.WorkSites.SingleOrDefault(w => w.Id == id);

            if (siteInDb == null)
                return NotFound();

            Mapper.Map<WorkSiteDTO, WorkSite>(workSiteDto, siteInDb);

            _context.SaveChanges();

            return Ok();
        }

        // DELET /api/worksites/1
        [HttpDelete]
        public IHttpActionResult DeleteWorkSite(int id)
        {
            var siteInDb = _context.WorkSites.SingleOrDefault(w => w.Id == id);

            if (siteInDb == null)
                return NotFound();

            _context.WorkSites.Remove(siteInDb);
            _context.SaveChanges();

            return Ok();
        }
    }
}
