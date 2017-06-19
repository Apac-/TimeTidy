using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity;
using System.Web.Http;
using TimeTidy.Models;
using TimeTidy.Models.DTOs;

namespace TimeTidy.Controllers.Api
{
    [Authorize(Roles = RoleName.CanManageWorkSites)]
    public class UserTimeSheetsController : ApiController
    {
        private ApplicationDbContext _context;

        public UserTimeSheetsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/usertimesheets/user_id
        public IHttpActionResult GetUserTimeSheets(string id)
        {
            var timeSheets = _context.TimeSheets
                .Include(s => s.SiteLocation)
                .Include(s => s.LogOnLocation)
                .Include(s => s.LogOffLocation)
                .Where(s => s.ApplicationUserId == id).ToList();

            List<TimeSheetDTO> dto = new List<TimeSheetDTO>();

            foreach (var sheet in timeSheets)
            {
                dto.Add(new TimeSheetDTO(sheet));
            }

            return Ok(dto);
        }

        // DELET /api/userTimeSheets/sheet_id
        [HttpDelete]
        public IHttpActionResult DeleteTimeSheet(int id)
        {
            var sheetInDb = _context.TimeSheets.SingleOrDefault(s => s.Id == id);

            if (sheetInDb == null)
                return NotFound();

            _context.TimeSheets.Remove(sheetInDb);
            _context.SaveChanges();

            return Ok();
        }
    }
}
