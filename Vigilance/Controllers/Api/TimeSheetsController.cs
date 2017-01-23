using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Vigilance.Models;
using Vigilance.Models.DTOs;

namespace Vigilance.Controllers.Api
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
            if (sheets.Count > 1) {
                foreach (TimeSheet timeSheet in sheets) {
                    if (returnSheet == null)
                        returnSheet = timeSheet;
                    else if (returnSheet.LogOnTime < timeSheet.LogOnTime)
                        returnSheet = timeSheet;
                }
            }

            DateTime? logonTime = null;
            if (returnSheet != null)
                logonTime = returnSheet.LogOnTime;

            var dto = new LogOnTimeDTO() {
                DateTime = logonTime
            };

            return Ok(dto);
        }
    }
}
