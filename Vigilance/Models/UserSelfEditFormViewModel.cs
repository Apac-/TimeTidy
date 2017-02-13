using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Vigilance.Models {
    public class UserSelfEditFormViewModel
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserSelfEditFormViewModel(ApplicationUser user) 
        {
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }

    }
}