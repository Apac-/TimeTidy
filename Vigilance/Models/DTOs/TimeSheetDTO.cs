using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vigilance.Models.DTOs {
    public class TimeSheetDTO {

        public string SiteName { get; set; }

        public LatLng SiteLocation { get; set; }

        public string SiteAddress { get; set; }

        public DateTime LogOnTime { get; set; }

        public LatLng LogOnLocation { get; set; }

        public DateTime? LogOffTime { get; set; }

        public LatLng LogOffLocation { get; set; }

        public TimeSheetDTO(TimeSheet sheet)
        {
            SiteName = sheet.SiteName;
            SiteLocation = sheet.SiteLocation;
            SiteAddress = sheet.SiteAddress;
            LogOnTime = sheet.LogOnTime;
            LogOnLocation = sheet.LogOnLocation;
            LogOffTime = sheet.LogOffTime;
            LogOffLocation = sheet.LogOffLocation;
        }
    }
}