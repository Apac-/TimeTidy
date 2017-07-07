﻿using System;
using System.Web.Http;
using AutoMapper;
using TimeTidy.DTOs;
using TimeTidy.Models;
using TimeTidy.Persistence;

namespace TimeTidy.Controllers.Api
{
    public class WorkSitesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkSitesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        public IHttpActionResult CreateWorkSite(WorkSiteDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (string.IsNullOrEmpty(dto.Name)
                || dto.Lat == 0 || dto.Lng == 0)
                return BadRequest();

            var workSite = _mapper.Map<WorkSiteDTO, WorkSite>(dto);

            _unitOfWork.WorkSites.Add(workSite);
            _unitOfWork.Complete();

            dto.Id = workSite.Id;

            return Created(new Uri(Request.RequestUri + "/" + workSite.Id), dto);
        }

        // PUT /api/worksites/1
        [HttpPut]
        [Authorize(Roles = RoleName.CanManageWorkSites)]
        public IHttpActionResult UpdateWorkSite(int id, WorkSiteDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var siteInDb = _unitOfWork.WorkSites.GetWorkSite(id);

            if (siteInDb == null)
                return NotFound();

            siteInDb = _mapper.Map<WorkSite>(dto);

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
