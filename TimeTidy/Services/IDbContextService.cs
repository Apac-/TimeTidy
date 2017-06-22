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
        ApplicationUser FindUser(string id);
        WorkSite FindWorkSite(int id);
    }
}
