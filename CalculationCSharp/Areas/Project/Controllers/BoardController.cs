using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using CalculationCSharp.Models;
using System.Net;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class BoardController : Controller
    {
        private CalculationDBContext db = new CalculationDBContext();

        // GET: Project/ProjectBoards
        public ActionResult Index()
        {

            ViewData["H1"] = "Project Management";
            ViewData["P1"] = "";

            return View();
        }

        [HttpGet]
        public ActionResult Board(int? id)
        {
            return View();

        }

    }
}