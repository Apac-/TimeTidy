using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TimeTidy.Models {
    public class UserSelfEditFormViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public UserSelfEditFormViewModel(ApplicationUser user) 
        {
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }

        public UserSelfEditFormViewModel()
        {
            
        }
    }
}