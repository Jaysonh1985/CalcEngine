using CalculationCSharp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Maintenance.Controllers
{
    public class MaintenanceController : Controller
    {
        CalculationCSharp.Models.ApplicationDbContext context = new CalculationCSharp.Models.ApplicationDbContext();
        // GET: Maintenance/Maintenance
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            if (!userManager.IsInRole(User.Identity.GetUserId(), "System Admin"))
            {
                ViewBag.Authenticated = false;
            }
            else
            {
                ViewBag.Authenticated = true;
            }
            

            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
          
        }
    }
}