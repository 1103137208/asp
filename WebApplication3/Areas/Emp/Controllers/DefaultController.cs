using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication3.Areas.Emp.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Emp/Default
        public ActionResult Index()
        {
            ViewBag.Desc = "hellow emp";
            return View();
        }
    }
}