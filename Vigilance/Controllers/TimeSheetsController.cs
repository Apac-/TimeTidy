using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vigilance.Controllers
{
    public class TimeSheetsController : Controller
    {
        // GET: TimeSheets
        public ActionResult Index()
        {
            return View();
        }
    }
}