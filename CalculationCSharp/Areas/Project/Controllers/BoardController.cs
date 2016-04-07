using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using CalculationCSharp.Models;


namespace CalculationCSharp.Areas.Project.Controllers
{
    public class BoardController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }                      
    }
}