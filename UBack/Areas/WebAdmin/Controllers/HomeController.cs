using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBLib;
using CommonTools;

namespace UBack.Areas.WebAdmin.Controllers
{
    public class HomeController : Controller
    {
        // GET: WebAdmin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}