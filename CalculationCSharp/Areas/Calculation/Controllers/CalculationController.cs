// Copyright (c) 2016 Project AIM
using CalculationCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalculationCSharp.Areas.Calculation.Controllers
{
    public class CalculationController : Controller
    {
        /// <summary>Controller for displaying the calculation release pages
        /// </summary>
        private CalculationDBContext db = new CalculationDBContext();
        // GET: Calculation/CalcReleasePage
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
        }

        // GET: Calculation/CalcReleasePage
        public ActionResult Form(int? id)
        {
            if (Request.IsAuthenticated)
            {
                CalcRelease ProjectBoard = db.CalcRelease.Find(id);
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