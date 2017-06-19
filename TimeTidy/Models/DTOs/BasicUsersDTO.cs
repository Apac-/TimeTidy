using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTidy.Models.DTOs {
    public class BasicUsersDTO {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}