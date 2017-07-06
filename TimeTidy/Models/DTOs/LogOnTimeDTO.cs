using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTidy.Models.DTOs {
    public class LogOnTimeDTO {
        public DateTime? DateTime { get; set; }

        public int? TimeSheetId { get; set; }

        public override bool Equals(object obj)
        {
            LogOnTimeDTO dto = obj as LogOnTimeDTO;

            if (obj == null)
                return false;

            if (dto.DateTime == this.DateTime && dto.TimeSheetId == this.TimeSheetId)
                return true;
            else
                return false;
        }
    }
}