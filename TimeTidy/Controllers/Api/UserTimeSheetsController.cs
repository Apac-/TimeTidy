using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data.Entity;
using System.Web.Http;
using TimeTidy.Models;
using TimeTidy.Models.DTOs;
using TimeTidy.Persistence;

namespace TimeTidy.Controllers.Api
{
    [Authorize(Roles = RoleName.CanManageWorkSites)]
    public class UserTimeSheetsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserTimeSheetsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET /api/usertimesheets/user_id
        public IHttpActionResult GetUserTimeSheets(string id)
        {
            var timeSheetsInDb = _unitOfWork.TimeSheets.GetTimeSheetsByUser(id);

            List<TimeSheetDTO> dto = new List<TimeSheetDTO>();

            foreach (var sheet in timeSheetsInDb)
            {
                dto.Add(new TimeSheetDTO(sheet));
            }

            return Ok(dto);
        }

        // DELETE /api/userTimeSheets/sheet_id
        [HttpDelete]
        public IHttpActionResult DeleteTimeSheet(int id)
        {
            var sheetInDb = _unitOfWork.TimeSheets.GetTimeSheet(id);

            if (sheetInDb == null)
                return NotFound();

            _unitOfWork.TimeSheets.Remove(sheetInDb);

            _unitOfWork.Complete();

            return Ok();
        }
    }
}
