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
using TimeTidy.Persistence;

namespace TimeTidy.Controllers.Api
{
    [Authorize(Roles = RoleName.CanManageWorkSites)]
    public class WorkSiteTimeSheetsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkSiteTimeSheetsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET /api/worksitetimesheets/1
        // <param name="id">WorkSite id</param>
        public IHttpActionResult GetWorkSiteTimeSheets(int id)
        {
            var timeSheets = _unitOfWork.TimeSheets.GetTimeSheetsByWorkSite(id);

            List<WorkSiteTimeSheetDTO> dto = new List<WorkSiteTimeSheetDTO>();

            foreach (var sheet in timeSheets)
            {
                dto.Add(new WorkSiteTimeSheetDTO(sheet));
            }

            return Ok(dto);
        }
    }
}
