using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTidy.Models {
    public class LatLng {
        public int Id { get; set; }
        public float? Lat { get; set; }
        public float? Lng { get; set; }

        public LatLng()
        {
        }

        public LatLng(float lat, float lng)
        {
            Lat = lat;
            Lng = lng;
        }
    }
}