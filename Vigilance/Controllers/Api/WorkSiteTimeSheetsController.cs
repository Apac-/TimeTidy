using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using TimeTidy.Models;
using TimeTidy.Models.DTOs;

namespace TimeTidy.Controllers.Api
{
    [Authorize(Roles = RoleName.CanManageWorkSites)]
    public class WorkSiteTimeSheetsController : ApiController
    {
        private ApplicationDbContext _context;

        public WorkSiteTimeSheetsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/worksitetimesheets/1
        public IHttpActionResult GetWorkSiteTimeSheets(int id)
        {
            var timeSheets = _context.TimeSheets
                .Include(s => s.SiteLocation)
                .Include(s => s.LogOnLocation)
                .Include(s => s.LogOffLocation)
                .Where(s => s.WorkSiteId == id).ToList();

            List<WorkSiteTimeSheetDTO> dto = new List<WorkSiteTimeSheetDTO>();

            foreach (var sheet in timeSheets)
            {
                dto.Add(new WorkSiteTimeSheetDTO(sheet));
            }

            return Ok(dto);
        }
    }
}
