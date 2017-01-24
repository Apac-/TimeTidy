using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vigilance.Models;
using Vigilance.Models.DTOs;

namespace Vigilance.Controllers.Api
{
    public class UserTimeSheetsController : ApiController
    {
        private ApplicationDbContext _context;

        public UserTimeSheetsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/timesheets/user_id
        public IHttpActionResult GetUserTimeSheets(string id)
        {
            var timeSheets = _context.TimeSheets.Where(s => s.ApplicationUserId == id);

            List<TimeSheetDTO> dto = new List<TimeSheetDTO>();

            foreach (var sheet in timeSheets)
            {
                dto.Add(new TimeSheetDTO(sheet));
            }

            return Ok(dto);
        }
    }
}
