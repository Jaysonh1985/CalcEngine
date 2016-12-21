using CalculationCSharp.Controllers;
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
    public class UserRoleMaintenanceController : Controller
    {
        CalculationCSharp.Models.ApplicationDbContext context = new CalculationCSharp.Models.ApplicationDbContext();
        private CalculationDBContext db = new CalculationDBContext();
        // GET: Maintenance/UserRoleMaintenance
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                if (!userManager.IsInRole(User.Identity.GetUserId(), "System Admin"))
                {
                    return RedirectToAction("AccessBlock", "Account", new { area = "" });

                }
                else
                {
                    // prepopulat roles for the view dropdown
                    var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                    var schemeList = db.Schemes.Select(m => new  { Value = m.Name, Text = m.Name }).Distinct().ToList();
                    ViewBag.Roles = list;
                    var listUsers = context.Users.OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
                    ViewBag.Users = listUsers;
                    ViewBag.SchemeList = new MultiSelectList(schemeList, "Value", "Text");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }
        //POST: Maintenance/UserRoleMaintenance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessageAdd = "Role created successfully";

            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            var schemeList = db.Schemes.Select(m => new { Value = m.Name, Text = m.Name }).Distinct().ToList();
            ViewBag.Roles = list;
            var listUsers = context.Users.OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
            ViewBag.Users = listUsers;
            ViewBag.SchemeList = new MultiSelectList(schemeList, "Value", "Text");

            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);
                // prepopulat roles for the view dropdown
                var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                var schemeList = db.Schemes.Select(m => new { Value = m.Name, Text = m.Name }).Distinct().ToList();
                ViewBag.Roles = list;
                var listUsers = context.Users.OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
                ViewBag.Users = listUsers;
                ViewBag.SchemeList = new MultiSelectList(schemeList, "Value", "Text");
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (userManager.IsInRole(user.Id, RoleName))
            {
                userManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessageDelete = "Role removed from this user successfully";
            }
            else
            {
                ViewBag.ResultMessageDelete = "This user doesn't belong to the selected role";
            }
            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            var schemeList = db.Schemes.Select(m => new { Value = m.Name, Text = m.Name }).Distinct().ToList();
            ViewBag.Roles = list;
            var listUsers = context.Users.OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
            ViewBag.Users = listUsers;
            ViewBag.SchemeList = new MultiSelectList(schemeList, "Value", "Text");
            return View("Index");
        }

        //POST: Maintenance/UserRoleMaintenance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SchemeAddToUser(string UserName, string[] SchemeName)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser user = userManager.FindByNameAsync(UserName).Result;
            user.Scheme = string.Join(",", SchemeName);
            userManager.Update(user);
            ViewBag.ResultMessageAddScheme = "Scheme's added to User successfully";
            // prepopulat roles for the view dropdown
            var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            var schemeList = db.Schemes.Select(m => new { Value = m.Name, Text = m.Name }).Distinct().ToList();
            ViewBag.Roles = list;
            var listUsers = context.Users.OrderBy(r => r.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName.ToString(), Text = rr.UserName }).ToList();
            ViewBag.Users = listUsers;
            ViewBag.SchemeList = new MultiSelectList(schemeList, "Value", "Text");
            return View("Index");
        }
    }
}