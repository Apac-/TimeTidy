using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTidy.Models;

namespace TimeTidy.Services
{
    public interface IDbContextService
    {
        ApplicationUser FindUserOrDefault(string id);
        ApplicationUser FindUser(string id);
        WorkSite FindWorkSiteOrDefault(int id);
        WorkSite FindWorkSite(int id);
        List<ApplicationUser> Users();
        List<IdentityRole> Roles();
        int SaveChanges();

        void Dispose();
    }
}
