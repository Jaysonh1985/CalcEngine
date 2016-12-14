// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using CalculationCSharp.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using CalculationCSharp.Areas.Project.Models;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Xml;
using System.Text;

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

            ProjectBoard ProjectBoard = db.ProjectBoard.Find(Convert.ToInt32(id));

            if(ProjectBoard == null)
            {
                ViewData["H1"] = "New Board";
            }
            else
            {
                ViewData["H1"] = ProjectBoard.Name;

            }

            return View();

        }


        

    }
}