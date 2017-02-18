using CalculationCSharp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Configuration.Controllers
{
    public class FunctionController : Controller
    {

        /// <summary>Controller for calculation configuration to view the views pages.
        /// </summary>
        CalculationCSharp.Models.ApplicationDbContext context = new CalculationCSharp.Models.ApplicationDbContext();
        private CalculationDBContext db = new CalculationDBContext();
        // GET: Project/ProjectConfigs
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (Request.IsAuthenticated)
            {

                ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;

                if (!userManager.IsInRole(User.Identity.GetUserId(), "Configuration") && !userManager.IsInRole(User.Identity.GetUserId(), "System Admin"))
                {
                    return RedirectToAction("AccessBlock", "Account", new { area = "" });
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        [HttpGet]
        public ActionResult Function(int id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (Request.IsAuthenticated)
            {
                if (!userManager.IsInRole(User.Identity.GetUserId(), "Configuration") && !userManager.IsInRole(User.Identity.GetUserId(), "System Admin"))
                {
                    return RedirectToAction("AccessBlock", "Account", new { area = "" });
                }
                else
                {
                    CalcFunctions CalcFunction = db.CalcFunctions.Find(Convert.ToInt32(id));
                    var List = db.UserSession.Where(i => i.Record == id);
                    if (List.Count() == 0)
                    {
                        UserSession UsersessionAdd = new UserSession();
                        UsersessionAdd.Section = "Function";
                        UsersessionAdd.Record = id;
                        UsersessionAdd.StartTime = DateTime.Now;
                        UsersessionAdd.Username = HttpContext.User.Identity.Name.ToString();
                        db.UserSession.Add(UsersessionAdd);
                        db.SaveChanges();
                    }

                    try
                    {
                        ViewData["SchemeName"] = CalcFunction.Scheme;
                        ViewData["CalcName"] = CalcFunction.Name;

                    }
                    catch
                    {

                    }
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        [HttpGet]
        public ActionResult Exit(int? id)
        {
            if (Request.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                if (!userManager.IsInRole(User.Identity.GetUserId(), "Configuration") && !userManager.IsInRole(User.Identity.GetUserId(), "System Admin"))
                {
                    return RedirectToAction("AccessBlock", "Account", new { area = "" });
                }
                else
                {
                    var List = db.UserSession.Where(i => i.Record == id);
                    UserSession UsersessionList = List.First();
                    db.UserSession.Remove(UsersessionList);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

        }
    }
}