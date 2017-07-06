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

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            BasicUsersDTO dto = obj as BasicUsersDTO;

            if (dto.UserId == UserId
                && dto.UserName == UserName
                && dto.FirstName == FirstName
                && dto.LastName == LastName)
                return true;
            else
                return false;
        }
    }
}