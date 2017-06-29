using System;
using System.Collections.Generic;

namespace TimeTidy.Models
{
    public class UserFormViewModel
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<string> UserRoles { get; set; }
        public IEnumerable<string> AvilableRoles { get; set; }

        public UserFormViewModel(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> userRoles)
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