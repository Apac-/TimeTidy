using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTidy.Services
{
    public class ApplicationUserManagerService : IApplicationUserManagerService
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserManagerService(ApplicationUserManager applicationUserManager)
        {
            _userManager = applicationUserManager;
        }
    }
}