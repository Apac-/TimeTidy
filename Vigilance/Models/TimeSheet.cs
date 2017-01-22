using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vigilance.Models {
    public class TimeSheet {
        public int Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        public WorkSite WorkSite { get; set; }

        [Required]
        public DateTime LogOnTime { get; set; }

        public LatLng LogOnLatLng { get; set; }

        public DateTime? LogOffTime { get; set; }

        public LatLng LogOffLatLng { get; set; }
    }
}