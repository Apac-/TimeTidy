﻿using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TimeTidy.Models;
using TimeTidy.Models.DTOs;
using TimeTidy.Persistance;

namespace TimeTidy.Controllers.Api
{
    public class TimeSheetsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TimeSheetsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET /api/timesheets/1
        public IHttpActionResult GetTimeSheets(int id)
        {
            string userId = User.Identity.GetUserId();

            // TODO (Jeff): C. Remove code later
            //var sheets = _context.TimeSheets
            //    .Where(s => s.ApplicationUserId == userId)
            //    .Where(s => s.WorkSiteId == id)
            //    .Where(s => s.LogOffTime == null)
            //    .ToList();

            var returnSheet = _unitOfWork.TimeSheets.GetMostRecentSheetByUser(userId, id);

            DateTime? logonTime = null;
            int? timeSheetId = null;

            if (returnSheet != null)
            {
                logonTime = returnSheet.LogOnTime;
                timeSheetId = returnSheet.Id;
            }

            var dto = new LogOnTimeDTO() {
                DateTime = logonTime,
                TimeSheetId = timeSheetId
            };

            return Ok(dto);
        }

        // POST /api/timesheets
        [HttpPost]
        public IHttpActionResult CreateTimeSheet(TimeSheetLogonDTO logonDto)
        {
            string userId = User.Identity.GetUserId();

            var timeSheet = new TimeSheet()
            {
                ApplicationUserId = userId,
                WorkSiteId = logonDto.WorkSiteId,
                SiteName = logonDto.SiteName,
                SiteLocation = new LatLng(logonDto.SiteLat, logonDto.SiteLng),
                SiteAddress = logonDto.SiteAddress,
                LogOnTime = DateTime.UtcNow,
                LogOnLocation = new LatLng(logonDto.UserLat, logonDto.UserLng)
            };

            _unitOfWork.TimeSheets.Add(timeSheet);

            _unitOfWork.Complete();

            return Ok();
        }

        // PUT /api/timesheets/id
        [HttpPut]
        public IHttpActionResult UpdateTimeSheet(int id, TimeSheetLogoffDTO logoffDto)
        {
            var timeSheet = _unitOfWork.TimeSheets.GetTimeSheet(id);

            if (timeSheet == null)
                return NotFound();

            if (timeSheet.ApplicationUserId != User.Identity.GetUserId())
                return BadRequest();

            timeSheet.LogOffTime = DateTime.UtcNow;
            timeSheet.LogOffLocation = new LatLng(logoffDto.UserLat, logoffDto.UserLng);

            _unitOfWork.Complete();

            return Ok();
        }
    }
}
