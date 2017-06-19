using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTidy.Models.DTOs {
    public class LogOnTimeDTO {
        public DateTime? DateTime { get; set; }
        public int? TimeSheetId { get; set; }
    }
}