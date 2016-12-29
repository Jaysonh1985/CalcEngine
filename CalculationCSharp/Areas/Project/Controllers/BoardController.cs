// Copyright (c) 2016 Project AIM
using System;
using System.Web.Mvc;
using CalculationCSharp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CalculationCSharp.Areas.Project.Controllers
{
    public class BoardController : Controller
    {
        private CalculationDBContext db = new CalculationDBContext();
        /// <summary>Controller for calculation configuration to view the views pages.
        /// </summary>
        CalculationCSharp.Models.ApplicationDbContext context = new CalculationCSharp.Models.ApplicationDbContext();
        // GET: Project/ProjectBoards
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (Request.IsAuthenticated)
            {
                ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
                if (!userManager.IsInRole(User.Identity.GetUserId(), "Project") && !userManager.IsInRole(User.Identity.GetUserId(), "System Admin"))
                {
                    return RedirectToAction("AccessBlock", "Account", new { area = "" });
                }
                else
                {
                    ViewData["H1"] = "Project Management";
                    ViewData["P1"] = "";
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        [HttpGet]
        public ActionResult Board(int? id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (Request.IsAuthenticated)
            {
                ApplicationUser user = userManager.FindByNameAsync(User.Identity.Name).Result;
                if (!userManager.IsInRole(User.Identity.GetUserId(), "Project") && !userManager.IsInRole(User.Identity.GetUserId(), "System Admin"))
                {
                    return RedirectToAction("AccessBlock", "Account", new { area = "" });
                }
                else
                {
                    ProjectBoard ProjectBoard = db.ProjectBoard.Find(Convert.ToInt32(id));
                    if (ProjectBoard == null)
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
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }      
    }
}