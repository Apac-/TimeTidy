using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TimeTidy.Models {
    public class UserFormViewModel
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<IdentityRole> UserRoles { get; set; }
        public IEnumerable<IdentityRole> AvilableRoles { get; set; }

        public UserFormViewModel(ApplicationUser user, IEnumerable<IdentityRole> roles, IEnumerable<IdentityRole> userRoles) 
        {
            UserId = user.Id;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;

            UserRoles = userRoles;

            AvilableRoles = roles;
        }


    }
}