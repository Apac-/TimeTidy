using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTidy.Models.DTOs {
    public class TimeSheetDTO {

        public int Id { get; set; }

        public string SiteName { get; set; }

        public LatLng SiteLocation { get; set; }

        public string SiteAddress { get; set; }

        public DateTime LogOnTime { get; set; }

        public LatLng LogOnLocation { get; set; }

        public DateTime? LogOffTime { get; set; }

        public LatLng LogOffLocation { get; set; }

        public TimeSheetDTO(TimeSheet sheet)
        {
            Id = sheet.Id;
            SiteName = sheet.SiteName;
            SiteLocation = sheet.SiteLocation;
            SiteAddress = sheet.SiteAddress;
            LogOnTime = sheet.LogOnTime;
            LogOnLocation = sheet.LogOnLocation;
            LogOffTime = sheet.LogOffTime;
            LogOffLocation = sheet.LogOffLocation;
        }

        public override bool Equals(object obj)
        {
            TimeSheetDTO dto = obj as TimeSheetDTO;

            if (dto == null)
                return false;

            if (dto.Id == Id && dto.SiteName == SiteName && dto.SiteLocation == SiteLocation
                && dto.SiteAddress == SiteAddress && dto.LogOnTime == LogOnTime && dto.LogOnLocation == LogOnLocation
                && dto.LogOffTime == LogOffTime && dto.LogOffLocation == LogOffLocation)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}