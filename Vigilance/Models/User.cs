﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vigilance.Models {
    public class User {
        public string Id { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<string> UserRoles { get; set; }

    }
}