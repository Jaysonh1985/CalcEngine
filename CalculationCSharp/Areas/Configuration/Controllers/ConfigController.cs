// Copyright (c) 2016 Project AIM
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using CalculationCSharp.Models;
using CalculationCSharp.Areas.Configuration.Models;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Text;

namespace CalculationCSharp.Areas.Configuration.Controllers
{
    public class ConfigController : Controller
    {
        /// <summary>Controller for calculation configuration to view the views pages.
        /// </summary>
        private CalculationDBContext db = new CalculationDBContext();
        // GET: Project/ProjectConfigs
        public ActionResult Index()
        {
            if(Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            
        }

        [HttpGet]
        public ActionResult Config(int? id)
        {

            if (Request.IsAuthenticated)
            {
                CalcConfiguration ProjectBoard = db.CalcConfiguration.Find(Convert.ToInt32(id));
                try
                {
                    ViewData["SchemeName"] = ProjectBoard.Scheme;
                    ViewData["CalcName"] = ProjectBoard.Name;
                }
                catch
                {

                }
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

        }
    }
}