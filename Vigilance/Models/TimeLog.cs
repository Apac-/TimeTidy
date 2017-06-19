using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTidy.Models {
    public class TimeLog {
        public int Id { get; set; }

        public DateTime Time { get; set; }
        public LatLng Location { get; set; }
    }
}