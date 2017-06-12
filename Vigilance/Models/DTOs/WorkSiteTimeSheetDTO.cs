using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vigilance.Models.DTOs
{
    public class WorkSiteTimeSheetDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string UserLogin { get; set; }

        public string UserId { get; set; }

        public LatLng SiteLocation { get; set; }

        public DateTime LogOnTime { get; set; }

        public LatLng LogOnLocation { get; set; }

        public DateTime? LogOffTime { get; set; }

        public LatLng LogOffLocation { get; set; }

        public WorkSiteTimeSheetDTO(TimeSheet sheet)
        {
            Id = sheet.Id;
            UserName = sheet.ApplicationUser.LastName + ", " + sheet.ApplicationUser.FirstName;
            UserLogin = sheet.ApplicationUser.UserName;
            UserId = sheet.ApplicationUserId;
            SiteLocation = sheet.SiteLocation;
            LogOnTime = sheet.LogOnTime;
            LogOnLocation = sheet.LogOnLocation;
            LogOffTime = sheet.LogOffTime;
            LogOffLocation = sheet.LogOffLocation;
        }
    }
}