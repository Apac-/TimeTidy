using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vigilance.Models;

namespace Vigilance.ViewModels {
    public class UserTimeSheetsViewModel {

        public string SiteName { get; set; }

        public LatLng SiteLocation { get; set; }

        public string SiteAddress { get; set; }

        public DateTime LogOnTime { get; set; }

        public LatLng LogOnLocation { get; set; }

        public DateTime? LogOffTime { get; set; }

        public LatLng LogOffLocation { get; set; }
    }
}