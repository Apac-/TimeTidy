using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vigilance.Models {
    public class TimeSheet {
        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int ApplicationUserId { get; set; }

        [Required]
        public WorkSite WorkSite { get; set; }

        [Required]
        public DateTime LogOnTime { get; set; }

        [Required]
        public Tuple<float,float> LogOnLatLng { get; set; }

        public DateTime? LogOffTime { get; set; }
        public Tuple<float,float> LogOffLatLng { get; set; }
    }
}