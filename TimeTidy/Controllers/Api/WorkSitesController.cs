﻿using System;
using System.Web.Http;
using AutoMapper;
using TimeTidy.DTOs;
using TimeTidy.Models;
using TimeTidy.Persistance;

namespace TimeTidy.Controllers.Api
{
    public class WorkSitesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkSitesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET /api/worksites
        public IHttpActionResult GetWorkSites()
        {
            return Ok(_unitOfWork.WorkSites.GetWorkSites());
        }

        // GET /api/worksites/1
        public IHttpActionResult GetWorkSite(int id)
        {
            var worksiteInDb = _unitOfWork.WorkSites.GetWorkSite(id);

            if (worksiteInDb == null)
                return NotFound();

            return Ok(worksiteInDb);
        }

        // POST /api/worksites
        [HttpPost]
        [Authorize(Roles = RoleName.CanManageWorkSites)]
        public IHttpActionResult CreateWorkSite(WorkSiteDTO workSiteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var workSite = Mapper.Map<WorkSiteDTO, WorkSite>(workSiteDto);

            _unitOfWork.WorkSites.Add(workSite);
            _unitOfWork.Complete();

            workSiteDto.Id = workSite.Id;

            return Created(new Uri(Request.RequestUri + "/" + workSite.Id), workSiteDto);
        }

        // PUT /api/worksites/1
        [HttpPut]
        [Authorize(Roles = RoleName.CanManageWorkSites)]
        public IHttpActionResult UpdateWorkSite(int id, WorkSiteDTO workSiteDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var siteInDb = _unitOfWork.WorkSites.GetWorkSite(id);

            if (siteInDb == null)
                return NotFound();

            Mapper.Map<WorkSiteDTO, WorkSite>(workSiteDto, siteInDb);

            _unitOfWork.Complete();

            return Ok();
        }

        // DELET /api/worksites/1
        [HttpDelete]
        [Authorize(Roles = RoleName.CanManageWorkSites)]
        public IHttpActionResult DeleteWorkSite(int id)
        {
            var siteInDb = _unitOfWork.WorkSites.GetWorkSite(id);

            if (siteInDb == null)
                return NotFound();

            _unitOfWork.WorkSites.Remove(siteInDb);
            _unitOfWork.Complete();

            return Ok();
        }
    }
}
