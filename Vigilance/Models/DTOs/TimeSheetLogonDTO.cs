using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vigilance.Models.DTOs {
    public class TimeSheetLogonDTO {
        [Required]
        public int WorkSiteId { get; set; }

        [Required]
        public string SiteName { get; set; }

        public float SiteLat { get; set; }
        public float SiteLng { get; set; }

        public string SiteAddress { get; set; }

        public float UserLat { get; set; }
        public float UserLng { get; set; }
    }
}