using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TresEmpanadas.Models;

namespace TresEmpanadas.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Info(DetailError error)
        {
            return View(error);
        }
    }
}