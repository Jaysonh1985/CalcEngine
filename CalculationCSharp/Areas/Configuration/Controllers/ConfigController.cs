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

        // GET: Project/ProjectConfigs
        public ActionResult Index()
        {

            ViewData["H1"] = "Configuration";
            ViewData["P1"] = "";

            return View();
        }

       
        
    }
}