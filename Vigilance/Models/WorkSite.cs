using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vigilance.Models {
    public class WorkSite {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public string StreetAddress { get; set; }

        [Required]
        public float Lat { get; set; }
        [Required]
        public float Lng { get; set; }

        public int Radius { get; set; }
    }
}