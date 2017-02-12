﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Vigilance.Models {
    public class UserFormViewModel
    {
        private ApplicationUser userInDb;
        private List<IdentityRole> roles;

        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<IdentityUserRole> UserRoles { get; set; }
        public ICollection<IdentityRole> AvilableRoles { get; set; }

        public UserFormViewModel(ApplicationUser user, List<IdentityRole> roles) 
        {
            UserId = user.Id;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserRoles = user.Roles;

            AvilableRoles = roles;
        }

    }
}