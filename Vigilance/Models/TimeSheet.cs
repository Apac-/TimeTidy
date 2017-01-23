using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vigilance.Models {
    public class TimeSheet {
        public int Id { get; set; }

        public virtual ApplicationUser ApplicationUser{ get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public string SiteName { get; set; }

        [Required]
        public LatLng SiteLocation { get; set; }

        public string SiteAddress { get; set; }

        [Required]
        public DateTime LogOnTime { get; set; }

        public LatLng LogOnLocation { get; set; }

        public DateTime? LogOffTime { get; set; }

        public LatLng LogOffLocation { get; set; }
    }
}