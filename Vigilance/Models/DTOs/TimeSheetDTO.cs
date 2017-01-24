using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vigilance.Models.DTOs {
    public class TimeSheetDTO {

        public string SiteName { get; set; }

        public float SiteLat { get; set; }
        public float SiteLng { get; set; }

        public string SiteAddress { get; set; }

        public DateTime LogOnTime { get; set; }

        public float LogOnLat { get; set; }
        public float LogOnLng { get; set; }

        public DateTime? LogOffTime { get; set; }

        public float LogOffLat { get; set; }
        public float LogOffLng { get; set; }

        public TimeSheetDTO(TimeSheet sheet)
        {
            SiteName = sheet.SiteName;
            SiteLat = sheet.SiteLocation.Lat.Value;
            SiteLng = sheet.SiteLocation.Lng.Value;
            SiteAddress = sheet.SiteAddress;
            LogOnTime = sheet.LogOnTime;
            LogOnLat = sheet.LogOnLocation.Lat.Value;
            LogOnLng = sheet.LogOnLocation.Lng.Value;
            LogOffTime = sheet.LogOffTime;
            LogOffLat = sheet.LogOffLocation.Lat.Value;
            LogOffLng = sheet.LogOffLocation.Lng.Value;
        }
    }
}