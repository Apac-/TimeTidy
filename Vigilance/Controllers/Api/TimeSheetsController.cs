using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using TimeTidy.Models;
using TimeTidy.Models.DTOs;

namespace TimeTidy.Controllers.Api
{
    public class TimeSheetsController : ApiController
    {
        private ApplicationDbContext _context;

        public TimeSheetsController()
        {
            _context = new ApplicationDbContext();
        }



        // GET /api/timesheets/1
        public IHttpActionResult GetTimeSheets(int id)
        {
            // Get all timeSheets that have to do with siteId
            // Filter sheets by userId associated
            // Filter by sheets that don't have a logoff time
            // if (none found)
            // return dto with no logon time
            // else
            // return dto with logon time
            string userId = User.Identity.GetUserId();
            var sheets = _context.TimeSheets
                .Where(s => s.ApplicationUserId == userId)
                .Where(s => s.WorkSiteId == id)
                .Where(s => s.LogOffTime == null)
                .ToList();

            // TODO (Jeff): C. Need a solution to deal with multiple logged on time sheets.

            // Find most recent time sheet;
            TimeSheet returnSheet = null;
            if (sheets.Count > 0) {
                foreach (var timeSheet in sheets) {
                    if (returnSheet == null)
                        returnSheet = timeSheet;
                    else if (returnSheet.LogOnTime < timeSheet.LogOnTime)
                        returnSheet = timeSheet;
                }
            }

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

            _context.TimeSheets.Add(timeSheet);
            _context.SaveChanges();

            return Ok();
        }

        // PUT /api/timesheets/id
        [HttpPut]
        public IHttpActionResult UpdateTimeSheet(int id, TimeSheetLogoffDTO logoffDto)
        {
            var timeSheet = _context.TimeSheets.SingleOrDefault(s => s.Id == id);

            if (timeSheet == null)
                return NotFound();

            if (timeSheet.ApplicationUserId != User.Identity.GetUserId())
                return BadRequest();

            timeSheet.LogOffTime = DateTime.UtcNow;
            timeSheet.LogOffLocation = new LatLng(logoffDto.UserLat, logoffDto.UserLng);

            _context.SaveChanges();

            return Ok();
        }
    }
}
