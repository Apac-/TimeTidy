﻿using System;
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
        public IEnumerable<WorkSiteDTO> GetWorkSites()
        {
            return _context.WorkSites.ToList().Select(Mapper.Map<WorkSite, WorkSiteDTO>);
        }

        // GET /api/worksites/1
        public WorkSiteDTO GetWorkSite(int id)
        {
            var worksite = _context.WorkSites.SingleOrDefault(w => w.Id == id);

            if (worksite == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Mapper.Map<WorkSite, WorkSiteDTO>(worksite);
        }

        // POST /api/worksites
        [HttpPost]
        public WorkSiteDTO CreateWorkSite(WorkSiteDTO workSiteDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var workSite = Mapper.Map<WorkSiteDTO, WorkSite>(workSiteDto);

            _context.WorkSites.Add(workSite);
            _context.SaveChanges();

            workSiteDto.Id = workSite.Id;

            return workSiteDto;
        }

        // PUT /api/worksites/1
        [HttpPut]
        public void UpdateWorkSite(int id, WorkSiteDTO workSiteDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var siteInDb = _context.WorkSites.SingleOrDefault(w => w.Id == id);

            if (siteInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map<WorkSiteDTO, WorkSite>(workSiteDto, siteInDb);

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
