using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vigilance.Models {
    public class WorkSite {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public int Radius { get; set; }
    }
}