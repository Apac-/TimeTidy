using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vigilance.Models {
    public class TimeLog {
        public DateTime Time { get; set; }
        public LatLng Location { get; set; }
    }
}